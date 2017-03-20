using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float maxSpeed = 10.0f;
	public int maxHealth = 2000;
	public int currentHealth;

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

	//private Overlord overlord;
	private PlayerUI playerUI;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;

	private float currentSpeed = 0.0f;


	// Use this for initialization
	void Start () 
	{
		_rb = gameObject.GetComponent<Rigidbody2D>();
		//_col = gameObject.GetComponent<BoxCollider2D>();

		/*currentHealth = maxHealth;
		gunPos = new Vector3 (fRadius, 0.0f, 0.0f);
		gun.position = transform.position + gunPos + gunPosOffset;*/
	}
	
	// Update is called once per frame
	void Update () {
		//UpdateHealthBar();
		HandleInput();
	}

	private void HandleInput()
	{
		//get controller state
		//var fireState = gamepad.R2();
		/*controllerState.x = gamepad.Move_X();
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
		}*/

		//handle movement
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical"); 

		bool moveLeft 	= Input.GetKey(KeyCode.A);
		bool moveRight 	= Input.GetKey(KeyCode.D);
		bool aimUp 		= Input.GetKey(KeyCode.W);
		bool aimDown 	= Input.GetKey(KeyCode.S);


		if (moveHorizontal != 0.0f || moveVertical != 0.0f)
		{
			Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
			_rb.velocity = (movement * maxSpeed * .1f);
		}

		if(moveLeft)
		{
			//Vector3 movement = new Vector3(
			_rb.velocity = new Vector3(-maxSpeed, 0.0f, 0.0f);
		}



		// handle aiming (right stick)
		/*if (controllerStateR.magnitude > 0.2f) 
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
		}*/
	}

	/*IEnumerator FireRoutine(float duration)
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
	}*/
}
