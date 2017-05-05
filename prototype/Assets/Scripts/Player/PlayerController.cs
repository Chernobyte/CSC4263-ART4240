using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
	public Text cHealth;
	public Text currentWeaponTxt;

	//for shop
	public List<ShopItem> weapons;
	public int currentWeapon;

	//Audio
	public AudioSource defaultFireSound;
	public AudioSource bulletBlastSound;
	public AudioSource slitherSound;
	public AudioSource takeDamageSource;
	public AudioClip takeDamage1;
	public AudioClip takeDamage2;
	public AudioClip takeDamage3;
	public AudioClip takeDamage4;
	public AudioClip takeDamage5;
	public AudioClip takeDamage6;
	public AudioClip takeDamage7;
	public AudioClip takeDamage8;
	public AudioClip takeDamage9;
	public AudioClip takeDamage10;
	public AudioClip takeDamage11;
	public AudioClip takeDamage12;
	System.Random random = new System.Random ();
	private int randomTakeDamage;
	private int map;
	private int currentScene;

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
		currentScene = SceneManager.GetActiveScene().buildIndex;

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

		bool moveLeft, moveRight, cycleWeapon, aimUp, aimDown, fireKey, shopKey, moveUp, moveDown;

		// Player 1 controls
		if(isP1)
		{
			moveLeft 	= Input.GetKey(KeyCode.A);
			moveRight 	= Input.GetKey(KeyCode.D);
			moveUp = Input.GetKey (KeyCode.E);
			moveDown = Input.GetKey (KeyCode.Q);
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
			moveUp = Input.GetKey (KeyCode.O);
			moveDown = Input.GetKey (KeyCode.U);
			cycleWeapon = Input.GetKeyDown (KeyCode.Return); //Enter key
			aimUp 		= Input.GetKey(KeyCode.I);
			aimDown 	= Input.GetKey(KeyCode.K);
			fireKey		= Input.GetKey(KeyCode.Semicolon);
			shopKey 	= Input.GetKeyDown (KeyCode.Quote);
		}

		if (currentScene == 8) {
			Debug.Log("hello");
			if (moveLeft) {
				_rb.velocity = new Vector3 (-maxSpeed, 0.0f, 0.0f);
				if (!slitherSound.isPlaying) {
					slitherSound.UnPause ();
				}
			} else if (moveRight) {
				_rb.velocity = new Vector3 (maxSpeed, 0.0f, 0.0f);
				if (!slitherSound.isPlaying) {
					slitherSound.UnPause ();
				}
			} else if (moveUp) {
				_rb.velocity = new Vector3 (0.0f, maxSpeed, 0.0f);
				if (!slitherSound.isPlaying) {
					slitherSound.UnPause ();
				}
			} else if (moveDown) {
					_rb.velocity = new Vector3 (0.0f, -maxSpeed, 0.0f);
					if (!slitherSound.isPlaying) {
						slitherSound.UnPause ();
					}
				}
			}else {
			// left/right movement
			if (moveLeft) {
				_rb.velocity = new Vector3 (-maxSpeed, 0.0f, 0.0f);
				if (!slitherSound.isPlaying) {
					slitherSound.UnPause ();
				}
			} else if (moveRight) {
				_rb.velocity = new Vector3 (maxSpeed, 0.0f, 0.0f);
				if (!slitherSound.isPlaying) {
					slitherSound.UnPause ();
				}
			} 	
		
			if (!moveLeft && !moveRight) {
				slitherSound.Pause ();
			}
		}
		//cycle weapons
		/*if (cycleWeapon)
			currentWeapon = (currentWeapon + 1) % weapons.GetUpperBound (0);*/
		if (cycleWeapon) 
		{
			if (currentWeapon + 1 < weapons.Count)
				currentWeapon++;
			else
				currentWeapon = 0;

			currentWeaponTxt.text = weapons [currentWeapon].ToString ().Replace(" (ShopItem)", "");
		}

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
		randomTakeDamage = random.Next (1,13);
		if (randomTakeDamage == 1) {
			takeDamageSource.clip = takeDamage1;
		} else if (randomTakeDamage == 2) {
			takeDamageSource.clip = takeDamage2;
		} else if (randomTakeDamage == 3) {
			takeDamageSource.clip = takeDamage3;
		} else if (randomTakeDamage == 4) {
			takeDamageSource.clip = takeDamage4;
		} else if (randomTakeDamage == 5) {
			takeDamageSource.clip = takeDamage5;
		} else if (randomTakeDamage == 6) {
			takeDamageSource.clip = takeDamage6;
		} else if(randomTakeDamage == 7){
			takeDamageSource.clip = takeDamage7;
		} else if(randomTakeDamage == 8){
			takeDamageSource.clip = takeDamage8;
		} else if(randomTakeDamage == 9){
			takeDamageSource.clip = takeDamage9;
		} else if(randomTakeDamage == 10){
			takeDamageSource.clip = takeDamage10;
		} else if(randomTakeDamage == 11){
			takeDamageSource.clip = takeDamage11;
		} else{
			takeDamageSource.clip = takeDamage12;
		}
		takeDamageSource.Play();
		
		UpdateHealth ();
		//AccumulateCurrency ();
		currentCurrency += 5;

		cHealth.text = currentHealth + "/50";

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

	void OnCollisionEnter2D(Collision2D other){

		if(other.transform.tag == "MovingPlatform"){
			transform.parent = other.transform;
		}
	}
}