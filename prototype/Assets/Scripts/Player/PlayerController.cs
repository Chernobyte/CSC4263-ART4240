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
	public float fRadius = 1.0f;
	public int currentCurrency;

	//public GameObject bullet, shopPrefab;
	public Transform gun;
	public Vector3 gunPosOffset = new Vector3(0.0f, 0.0f, -0.1f); //use this to line up cursor with character's mouth/etc
	public Slider healthBar;
	public Text Currency; //needs to be moved either next to or under the healthbar
	//for shop
	public ShopItem[] weapons;
	public int currentWeapon;

	//Audio
	public AudioSource defaultFireSound;
	public AudioSource bulletBlastSound;
	public AudioSource slitherSound;

	private bool canFire = true;
	private bool shopOpen = false;
	private int shotCharge = 0;
	private float angle, aimRange, minAngle; //maxAngle = minAngle + aimRange
	private float currentSpeed = 0.0f;
	private float lastUpdate;

	private Overlord overlord;
	private Vector3 gunPos;
	private Vector3 charSize;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;
	private PlayerUI playerUI; //use this instead of slider
	private Transform spawnPoint;
	private ShopHandler shop;

	public Vector3 zeroVector = new Vector3(0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start () 
	{
		/*foreach(SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer> ()){
			Debug.Log (sr.name + " : " + sr.bounds.size);
		}*/

		_rb = gameObject.GetComponent<Rigidbody2D>();
		//_col = gameObject.GetComponent<BoxCollider2D>();
		shop = gameObject.GetComponent<ShopHandler>();

		InitializeHurtbox ();

		isDead = false;
		currentHealth = maxHealth;
		currentWeapon = 0;
		angle = 0.0f;
		minAngle = 0.0f;
		aimRange = 60.0f;
		lastUpdate = 0.0f;
		currentCurrency = 0;
		gunPos = new Vector3 (fRadius, 0.0f, 0.0f);
		gun.position = transform.position + gunPos + gunPosOffset;

		//this goes in Instantiate? 
		if(!isP1)
		{
			transform.Rotate(0, 180, 0, Space.Self);
			gunPosOffset.x *= -1;
		}

		slitherSound.Play ();
		slitherSound.Pause ();
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
		HitboxCallback hurtbox = GetComponentInChildren<HitboxCallback> ();
		hurtbox.Init (OnHitboxTriggerEnter, OnHitboxTriggerExit, this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//UpdateHealth();
		if (!isDead) 
		{
			HandleInput ();
			AccumulateCurrency ();
		}

	}

	private void HandleInput()
	{
		//handle movemement

		bool moveLeft, moveRight, cycleWeapon, aimUp, aimDown, fireKey, shopKey;

		// Player 1 controls
		if(isP1)
		{
			moveLeft 	= Input.GetKey(KeyCode.A);
			moveRight 	= Input.GetKey(KeyCode.D);
			cycleWeapon = Input.GetKeyDown (KeyCode.H);
			aimUp 		= Input.GetKey(KeyCode.W);
			aimDown 	= Input.GetKey(KeyCode.S);
			fireKey		= Input.GetKey(KeyCode.F);
			shopKey 	= Input.GetKeyDown (KeyCode.G);
		}
		// Player 2 controls (will i,j,k,l work better?)
		else
		{
			moveLeft 	= Input.GetKey(KeyCode.J);
			moveRight 	= Input.GetKey(KeyCode.L);
			cycleWeapon = Input.GetKeyDown (KeyCode.Return); //Enter key
			aimUp 		= Input.GetKey(KeyCode.I);
			aimDown 	= Input.GetKey(KeyCode.K);
			fireKey		= Input.GetKey(KeyCode.Semicolon);
			shopKey 	= Input.GetKeyDown (KeyCode.Quote);
		}
			
		// left/right movement
		if(moveLeft){
			_rb.velocity = new Vector3(-maxSpeed, 0.0f, 0.0f);
			if (!slitherSound.isPlaying) {
				slitherSound.UnPause ();
			}
		} 		
		else if(moveRight){
			_rb.velocity = new Vector3( maxSpeed, 0.0f, 0.0f);
			if (!slitherSound.isPlaying) {
				slitherSound.UnPause ();
			}
		} 	

		if (!moveLeft && !moveRight) {
			slitherSound.Pause ();
		}

		//cycle weapons
		/*if (cycleWeapon)
			currentWeapon = (currentWeapon + 1) % weapons.GetUpperBound (0);*/


		// handle aiming 
	
		//store angle to check if it has changed
		float oldAngle = angle;

		//adjust angle based on input
		if (!shopOpen) 
		{
			if (aimUp)
				angle += aimSpeed * Time.deltaTime;
			else if (aimDown)
				angle -= aimSpeed * Time.deltaTime;
		}

		// constrain to aiming range & change position/rotation if angle has changed
		if (oldAngle != angle) 
		{
			if (angle < minAngle)
				angle = minAngle;
			else if (angle > minAngle + aimRange)
				angle = minAngle + aimRange;

			// position & rotation of the cursor on fRadius
			gunPos = Quaternion.AngleAxis(angle, transform.forward) * (transform.right * fRadius);
			gun.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
			// apply the cursor's new position with an added offset for minor tweaking
			gun.position = transform.position + gunPos + gunPosOffset;
		}

		// fire handling
		if(fireKey && canFire && !shopOpen)
		{
				FireBullet ();
				canFire = false;
				//StartCoroutine (FireRoutine (fireRate));
				StartCoroutine (FireRoutine (weapons [currentWeapon].fireRate));
		}
			
		if (shopKey) 
		{
			shopOpen = !shopOpen;
			shop.enabled = !shop.enabled;
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

		GameObject curBullet = Instantiate (weapons[currentWeapon].bulletPrefab, bulletPos, gun.transform.rotation);
		curBullet.GetComponent<Bullet>().GetFiringPlayer(this);

		defaultFireSound.Play ();
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
		bulletBlastSound.Play ();
		UpdateHealth ();
		//AccumulateCurrency ();
		currentCurrency += 5;
	}

	private void AccumulateCurrency()
	{
		/*if(isP1){
			currentCurrency += 10;
			Currency.text = "Currency:" + currentCurrency;
		
		}
		if (!isP1) {
			currentCurrency += 10;
			Currency.text = "Currency:" + currentCurrency;
		}*/
		if (Time.time - lastUpdate >= 1f) 
		{
			currentCurrency += 1;
			Currency.text = "Currency: " + currentCurrency;
			lastUpdate = Time.time;
		}
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

	/*
	public void Winscreen(){

		if(isP1 && isDead && win.gameObject.activeInHierarchy==false){
			
			win.gameObject.SetActive (true);
			Time.timeScale = 0;
			WhoWins.text = "Player 2 Wins";


		}
		if(!isP1 && isDead && win.gameObject.activeInHierarchy==false){
			win.gameObject.SetActive (true);
			Time.timeScale = 0;
			WhoWins.text = "Player 1 Wins";
		}

	}
*/
}