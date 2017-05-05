using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxcurrentJumpCount = 2;
    public float jumpPreBuffer = 0.1f;
    public float jumpStrength = 20.0f;
    public float shortJumpStrength = 14.0f;
    public float airJumpStrength = 18.5f;
    public float wallJumpStrength = 18.5f;
    public float wallJumpHorizontalMultiplier = 1.3f;
    public float shortWallJumpStrength = 14.0f;
    public float shortWallJumpHorizontalMultiplier = 0.8f;
    public float gravity = -1.0f;
    public float maxFallSpeed = -10.0f;
    public float acceleration = 0.7f;
    public float deceleration = 1.0f;
    public float airDeceleration = 0.5f;
    public float maxSpeed = 10.0f;
    public int maxHealth = 2000;
    public int currentHealth;
    

    public GameObject topTriggerObject;
    public GameObject bottomTriggerObject;
    public GameObject leftTriggerObject;
    public GameObject rightTriggerObject;

    //for aiming
    public Transform gun;
    public float fRadius = 1.0f;
    public float bulletSpeed = 5.0f;
    public Vector3 gunPosOffset = new Vector3(0.0f, 0.0f, -0.1f); //use this to line up arm with character's shoulder
                                                                  //bullet
    public GameObject bullet;
    public float bulletSpawnOffset = 1.2f;
    public float fireRate = 1.0f;

    private float angle = 0.0f;
    private Vector3 gunPos = new Vector3(1.0f, 0.0f, 0.0f);
    private bool canFire = true;

    //ability stuff

    //private Overlord overlord;
    private PlayerUI playerUI;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _collider;

    private int playerNumber;
    private float currentSpeed = 0.0f;
    private float currentFallSpeed = 0.0f;

    private InputController gamepad;
    private Vector2 controllerState;
    private Vector2 controllerStateR; //Right stick state
    private bool canJump = false;
    private bool onWallLeft = false;
    private bool onWallRight = false;
    private bool canWallJumpToLeft = false;
    private bool canWallJumpToRight = false;
    private bool onGround = false;
    private bool fastFalling = false;
    private int currentJumpCount = 0;
    private float gravityForce;
    private float jumpPressedTime;
    private float jumpReleasedTime;
    private bool jumpBufferState = false;
    private bool wallJumpBufferState = false;
    private bool applyDecelerationThisTick;
    

    void Start() 
	{
		_rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _collider = gameObject.GetComponent<BoxCollider2D>();

        //InitializeTriggers();

        currentHealth = maxHealth;
		gunPos = new Vector3 (fRadius, 0.0f, 0.0f);
		gun.position = transform.position + gunPos + gunPosOffset;
    }

    /*private void InitializeTriggers()
    {
        topTriggerObject.GetComponent<TriggerCallback>().Init(OnTopTriggerEnter, OnTopTriggerEnter);
        bottomTriggerObject.GetComponent<TriggerCallback>().Init(OnBottomTriggerEnter, OnBottomTriggerExit);
        leftTriggerObject.GetComponent<TriggerCallback>().Init(OnLeftTriggerEnter, OnLeftTriggerExit);
        rightTriggerObject.GetComponent<TriggerCallback>().Init(OnRightTriggerEnter, OnRightTriggerExit);
    }
    
    void Update() 
	{
        UpdateHealthBar();
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleGravity();
        ApplySpeedToRigidBody();
    }

    public void init(int playerNumber, Overlord overlord, PlayerUI playerUI)
    {
        this.overlord = overlord;
        this.playerNumber = playerNumber;
        this.playerUI = playerUI;

        switch (playerNumber)
        {
            case 1: gamepad = new InputController(ControllerNumber.ONE); break;
            case 2: gamepad = new InputController(ControllerNumber.TWO); break;
            case 3: gamepad = new InputController(ControllerNumber.THREE); break;
            case 4: gamepad = new InputController(ControllerNumber.FOUR); break;
        }
    }

    private void ApplySpeedToRigidBody()
    {
        if (Mathf.Abs(currentSpeed) > Mathf.Abs(maxSpeed))
        {
            applyDecelerationThisTick = true;
        }

        if (applyDecelerationThisTick == true)
        {
            float actualDeceleration = deceleration;

            if (!onGround)
                actualDeceleration = airDeceleration;

            if (currentSpeed < 0)
            {
                if (-actualDeceleration < currentSpeed)
                {
                    currentSpeed = 0;
                }
                else
                {
                    currentSpeed += actualDeceleration;
                }
            }
            else if (currentSpeed > 0)
            {
                if (actualDeceleration > currentSpeed)
                {
                    currentSpeed = 0;
                }
                else
                {
                    currentSpeed -= actualDeceleration;
                }
            }
        }

        applyDecelerationThisTick = false;

        _rigidBody.velocity = new Vector2(currentSpeed, currentFallSpeed);
    }

    private void HandleGravity()
    {
        if (!onGround)
        {
            if (fastFalling)
            {
                currentFallSpeed = maxFallSpeed * 1.5f;
            }
            else
            {
                currentFallSpeed += gravity;

                if (onWallLeft || onWallRight)
                {
                    if (currentFallSpeed < maxFallSpeed / 2.0f)
                        currentFallSpeed = maxFallSpeed / 2.0f;
                }
                else
                {
                    if (currentFallSpeed < maxFallSpeed)
                        currentFallSpeed = maxFallSpeed;
                }
            }
        }
    }

    private void HandleJump()
    {
        var timeSinceJumpOrdered = Time.time - jumpPressedTime;
        var jumpButtonHeldLength = jumpReleasedTime - jumpPressedTime;

        if (onGround)
        {
            if (jumpBufferState && timeSinceJumpOrdered > jumpPreBuffer)
            {
                if (jumpButtonHeldLength < 0) // not yet released
                {
                    Jump();
                }
                else
                {
                    ShortJump();
                }

                jumpBufferState = false;
            }
        }
        else
        {
            if (wallJumpBufferState && timeSinceJumpOrdered > jumpPreBuffer)
            {
                WallJumpDirection direction;

                wallJumpBufferState = false;

                if (onWallLeft)
                {
                    direction = WallJumpDirection.Right;
                }
                else if (onWallRight)
                {
                    direction = WallJumpDirection.Left;
                }
                else
                {
                    return;
                }

                if (jumpButtonHeldLength < 0) // not yet released
                {
                    WallJump(direction);
                }
                else
                {
                    ShortWallJump(direction);
                }
            }
        }
    }

    private void HandleInput()
    {
        //get controller state
        controllerState.x = gamepad.Move_X();
        controllerState.y = gamepad.Move_Y();
        controllerStateR.x = gamepad.Aim_X();
        controllerStateR.y = gamepad.Aim_Y();
        
        var jumpInputReceived = gamepad.R1();
        var jumpInputStopped = gamepad.R1Up();
		var fireState = gamepad.R2();
        var ability1 = gamepad.L1();
        var ability2 = gamepad.L2();

        // Left Stick Right Tilt
        if (controllerState.x > 0.2)
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration;
                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;
            }
        }
        // Left Stick Left Tilt
        else if (controllerState.x < -0.2)
        {
            if (currentSpeed > -maxSpeed)
            {
                currentSpeed -= acceleration;
                if (currentSpeed < -maxSpeed)
                    currentSpeed = -maxSpeed;
            }
        }
        // No Left Stick X Input
        else
        {
            applyDecelerationThisTick = true;
        }

        // Left Stick Y Input
        if (controllerState.y < -0.8)
        {
            if (!onGround && !onWallLeft && !onWallRight)
            {
                fastFalling = true;
            }
        }

        if (jumpInputReceived)
        {
            jumpPressedTime = Time.time;

            if (canJump)
            {
                if (onWallLeft && !onGround && canWallJumpToRight)
                {
                    wallJumpBufferState = true;
                }
                else if (onWallRight && !onGround && canWallJumpToLeft)
                {
                    wallJumpBufferState = true;
                    
                }
                else if (currentJumpCount < maxcurrentJumpCount)
                {
                    if (onGround)
                    {
                        jumpBufferState = true;
                    }
                    else
                    {
                        AirJump();
                    }
                }
            }
        }

        if (jumpInputStopped)
        {
            jumpReleasedTime = Time.time;
        }

		// handle aiming (right stick)
		if (controllerStateR.magnitude > 0.2f) 
		{
			angle = Mathf.Atan2 (controllerStateR.y, controllerStateR.x) * Mathf.Rad2Deg;
			gunPos = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right * fRadius);
			//gun.position = transform.position + gunPos + gunPosOffset;

			// handle gun rotation (why the fuck is it gettign skewed? the scale doesnt change?)
			//because the player's y value for their scale is 2, numbnutz. and this passes down to the child
			//How to fix this without changing the player's x,y scale values to 1?
			gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		gun.position = transform.position + gunPos + gunPosOffset;

		if (fireState > 0.2f && canFire) 
		{
			FireWeapon ();
			canFire = false;
			StartCoroutine (FireRoutine (fireRate));
		}

        if(ability1)
        {
            Ability1();
        }
        if(ability2>.2)
        {
            Ability2();
        }
    }

	IEnumerator FireRoutine(float duration)
	{
		yield return new WaitForSeconds (duration);
		canFire = true;
	}

	private void FireWeapon()
	{
		GameObject curBullet = Instantiate (bullet, 
											gun.transform.position + (gun.transform.right * bulletSpawnOffset), 
											gun.transform.rotation);
		Rigidbody2D rb = curBullet.GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2(gun.transform.right.x, gun.transform.right.y) * bulletSpeed;
	}

    private void Ability1()
    {
        gameObject.GetComponent<AbilityOne>().fire(gun.transform);
    }

    private void Ability2()
    {

    }

    private void PreJump()
    {
        currentJumpCount++;
        fastFalling = false;
    }

    private void ShortJump()
    {
        PreJump();
        currentFallSpeed = shortJumpStrength;
    }

    private void Jump()
    {
        PreJump();
        currentFallSpeed = jumpStrength;
    }

    private void AirJump()
    {
        currentJumpCount++;
        fastFalling = false;
        canWallJumpToRight = true;
        canWallJumpToLeft = true;

        if (controllerState.x > 0.2 || controllerState.x < -0.2)
        {
            currentSpeed = maxSpeed * controllerState.x;
        }

        currentFallSpeed = airJumpStrength;
    }

    private enum WallJumpDirection { Left, Right };

    private void ShortWallJump(WallJumpDirection direction)
    {
        fastFalling = false;

        if (direction == WallJumpDirection.Right)
        {
            currentSpeed = maxSpeed * shortWallJumpHorizontalMultiplier;
            canWallJumpToRight = false;
            canWallJumpToLeft = true;
        }
        else if (direction == WallJumpDirection.Left)
        {
            currentSpeed = -maxSpeed * shortWallJumpHorizontalMultiplier;
            canWallJumpToLeft = false;
            canWallJumpToRight = true;
        }

        currentFallSpeed = shortWallJumpStrength;
    }

    private void WallJump(WallJumpDirection direction)
    {
        fastFalling = false;

        if (direction == WallJumpDirection.Right)
        {
            currentSpeed = maxSpeed * wallJumpHorizontalMultiplier;
            canWallJumpToRight = false;
            canWallJumpToLeft = true;
        }
        else if (direction == WallJumpDirection.Left)
        {
            currentSpeed = -maxSpeed * wallJumpHorizontalMultiplier;
            canWallJumpToLeft = false;
            canWallJumpToRight = true;
        }

        currentFallSpeed = wallJumpStrength;
    }

    public void OnTopTriggerEnter(Collider2D collision)
    {

    }

    public void OnTopTriggerExit(Collider2D collision)
    {

    }

    public void OnBottomTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = true;
            canJump = true;
            canWallJumpToLeft = true;
            canWallJumpToRight = true;
            wallJumpBufferState = true;
            currentJumpCount = 0;
            currentFallSpeed = 0;
            fastFalling = false;
        }
    }

    public void OnBottomTriggerExit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = false;
            currentJumpCount = 1;
            canJump = true;
        }
    }

    public void OnLeftTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWallLeft = true;
            fastFalling = false;
        }
    }

    public void OnLeftTriggerExit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWallLeft = false;
        }
    }

    public void OnRightTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWallRight = true;
            fastFalling = false;
        }
    }

    public void OnRightTriggerExit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onWallRight = false;
        }
    }

    private void UpdateHealthBar()
    {
        playerUI.UpdateHealthBar(currentHealth, maxHealth);
    }*/
}
