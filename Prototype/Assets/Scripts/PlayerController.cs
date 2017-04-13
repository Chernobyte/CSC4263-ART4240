using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	public bool isP1, isDead;
	public int maxHealth = 100;
	public int currentHealth;
	public float maxSpeed = 10.0f;
	public float aimSpeed = 100.0f;
	public float bulletSpawnOffset = 1.2f;
	public float fireRate = 1.0f;
	public float fRadius = 1.0f;
	//public float bulletForce = 5.0f;

	public GameObject bullet, shopPrefab;
	public Transform gun;
	public Vector3 gunPosOffset = new Vector3(0.0f, 0.0f, -0.1f); //use this to line up cursor with character's mouth/etc
	public Slider healthBar;

	private bool canFire = true;
	private bool shopOpen = false;
	private int shotCharge = 0;
	private float angle, aimRange, minAngle; //maxAngle = minAngle + aimRange
	private float currentSpeed = 0.0f;

	private Overlord overlord;
	private Vector3 gunPos;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;
	private PlayerUI playerUI; //use this instead of slider
	private Transform spawnPoint;

	// Use this for initialization
	void Start () 
	{
		_rb = gameObject.GetComponent<Rigidbody2D>();
		//_col = gameObject.GetComponent<BoxCollider2D>();

		InitializeHurtbox ();

		isDead = false;
		currentHealth = maxHealth;
		angle = 0.0f;
		minAngle = 0.0f;
		aimRange = 60.0f;
		gunPos = new Vector3 (fRadius, 0.0f, 0.0f);
		gun.position = transform.position + gunPos + gunPosOffset;

		//this goes in Instantiate 
		if(!isP1)
		{
			transform.Rotate(0, 180, 0, Space.Self);
			gunPosOffset.x *= -1;
		}
			
		// insantiate shop at fixed position above player
		shopPrefab = Instantiate(shopPrefab, transform.position + Vector3.up * 5, Quaternion.identity);
		shopPrefab.transform.SetParent(transform);
		shopPrefab.SetActive (false);

		//later, when more characters are added, we will have to map unique bullet prefabs if characters
		//will have different shot types.
	}

	//used in "overlord" to spawn characters after being selected
	public void Init(Overlord overlord, bool isP1, PlayerUI playerUI, Transform spawnPoint)
	{
		this.overlord = overlord;
		this.isP1 = isP1;
		this.playerUI = playerUI;
		this.spawnPoint = spawnPoint;

		//if(!isP1){ transform.Rotate(0, 180, 0, Space.Self); gunPosOffset.x *= -1; }
		//should be here?
	}

	private void InitializeHurtbox()
	{
		/*var hurtboxes = GetComponentsInChildren<HitboxCallback> ();
		foreach (var hurtbox in hurtboxes) 
		{
			hurtbox.Init (OnHitboxTriggerEnter, OnHitboxTriggerExit, this);
		}*/
		HitboxCallback hurtbox = GetComponentInChildren<HitboxCallback> ();
		hurtbox.Init (OnHitboxTriggerEnter, OnHitboxTriggerExit, this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//UpdateHealth();
		if(!isDead)
			HandleInput();
	}

	private void HandleInput()
	{
		//handle movemement

		bool moveLeft, moveRight, aimUp, aimDown, fireKey, shopKey;

		// Player 1 controls
		if(isP1)
		{
			moveLeft 	= Input.GetKey(KeyCode.A);
			moveRight 	= Input.GetKey(KeyCode.D);
			aimUp 		= Input.GetKey(KeyCode.W);
			aimDown 	= Input.GetKey(KeyCode.S);
			fireKey		= Input.GetKey(KeyCode.F);
			shopKey 	= Input.GetKeyDown (KeyCode.G);
		}
		// Player 2 controls (will i,j,k,l work better?)
		else
		{
			moveLeft 	= Input.GetKey(KeyCode.LeftArrow);
			moveRight 	= Input.GetKey(KeyCode.RightArrow);
			aimUp 		= Input.GetKey(KeyCode.UpArrow);
			aimDown 	= Input.GetKey(KeyCode.DownArrow);
			fireKey		= Input.GetKey(KeyCode.Semicolon);
			shopKey 	= Input.GetKeyDown (KeyCode.Quote);
		}
			
		// left/right movement
		if(moveLeft) 		_rb.velocity = new Vector3(-maxSpeed, 0.0f, 0.0f);
		else if(moveRight) 	_rb.velocity = new Vector3( maxSpeed, 0.0f, 0.0f);


		// handle aiming 
	
		//store angle to check if it has changed
		float oldAngle = angle;

		//adjust angle based on input
		if (aimUp) 
			angle += aimSpeed * Time.deltaTime;
		else if(aimDown)
			angle -= aimSpeed * Time.deltaTime;

		// constrain to aiming range & change position/rotation if angle has changed
		if (oldAngle != angle) 
		{
			if (angle < minAngle)
				angle = minAngle;
			else if (angle > minAngle + aimRange)
				angle = minAngle + aimRange;

			// position & rotation of the cursor on fRadius
			gunPos = Quaternion.AngleAxis(angle, transform.forward) * (transform.right * fRadius);
			gun.rotation = Quaternion.AngleAxis(angle, transform.forward);
			// apply the cursor's new position with an added offset for minor tweaking
			gun.position = transform.position + gunPos + gunPosOffset;
		}

		// charge shots?
		// if(Input.GetKeyUp(KeyCode.F)) shotCharge = 0;

		// fire handling
		if(fireKey && canFire)
		{
			FireBullet (); //damage should depend on shot type
			canFire = false;
			StartCoroutine (FireRoutine (fireRate));
		}
			
		if (shopKey) 
		{
			shopPrefab.SetActive (!shopPrefab.activeInHierarchy);
		}
	}

	IEnumerator FireRoutine(float duration)
	{
		yield return new WaitForSeconds (duration);
		canFire = true;
	}

	private void FireBullet()
	{
		Vector3 bulletPos = gun.transform.position + (gun.transform.right * bulletSpawnOffset);

		GameObject curBullet = Instantiate (bullet, bulletPos, gun.transform.rotation);

		//Vector2 direction = new Vector2 (bulletPos.x, bulletPos.y);

		//curBullet.GetComponent<Bullet> ().Initialize (direction, this);
		curBullet.GetComponent<Bullet>().GetFiringPlayer(this);
	}

	//should only check when someone takes damage
	private void UpdateHealth()
	{
		if(currentHealth <= 0)
		{
			Debug.Log ("DEAD");
			isDead = true;
			//currentHealth = maxHealth;
			//opponent.GetComponent<PlayerController>().currentHealth = maxHealth;
		}

		healthBar.value = currentHealth;
	}

	public void TakeDmg(int dmg)
	{
		currentHealth -= dmg;
		UpdateHealth ();
	}

	/*public void UpdateHealthBar()
	{
		playerUI.UpdateHealthBar(currentHealth, maxHealth);
	}*/

	public void OnHitboxTriggerEnter(Collider2D collision)
	{

	}

	public void OnHitboxTriggerExit(Collider2D collision)
	{

	}
}
