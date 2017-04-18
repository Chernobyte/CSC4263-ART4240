using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	public bool isP1;
	public int maxHealth = 100;
	public int currentHealth;
	public float maxSpeed = 10.0f;
	public float aimSpeed = 100.0f;
	public float bulletSpawnOffset = 1.2f;
	public float fireRate = 1.0f;
	public float fRadius = 1.0f;
	public float bulletForce = 5.0f;

	public GameObject bullet, opponent, shopPrefab;
	public Canvas canvas;
	public Transform gun;
	public Vector3 gunPosOffset = new Vector3(0.0f, 0.0f, -0.1f); //use this to line up cursor with character's mouth/etc

	private bool canFire = true;
	private bool shopOpen = false;
	private int shotCharge = 0;
	private float angle, aimRange, minAngle; //maxAngle = minAngle + aimRange
	private float currentSpeed = 0.0f;

	private Vector3 gunPos;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;
	private Slider healthBar;

	// Use this for initialization
	void Start () 
	{
		_rb = gameObject.GetComponent<Rigidbody2D>();
		//_col = gameObject.GetComponent<BoxCollider2D>();

		currentHealth = maxHealth;
		aimRange = 60.0f;

		if(isP1)
		{
			angle = 0.0f;
			gunPos = new Vector3 (fRadius, 0.0f, 0.0f);
			// range: [0,60]
			minAngle = 0.0f;
			healthBar = GameObject.Find("P1 Healthbar").GetComponent<Slider>();
		}
		else
		{
			angle = 180.0f;
			gunPos = new Vector3 (-fRadius, 0.0f, 0.0f);
			// range: [120,180]
			minAngle = 120.0f;
			healthBar = GameObject.Find("P2 Healthbar").GetComponent<Slider>();
		}
		gun.position = transform.position + gunPos + gunPosOffset;

		//later, when more characters are added, we will have to map unique bullet prefabs if characters
		//will have different shot types.
	}
	
	// Update is called once per frame
	void Update () {
		UpdateHealth();
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
		if (aimUp) 
		{
			// counterclockwise
			if (isP1) angle += aimSpeed * Time.deltaTime;

			// constrain to aiming range
			if (angle < minAngle) angle = minAngle;
			else if (angle > minAngle + aimRange) angle = minAngle + aimRange;
		}
		else if(aimDown)
		{
			// clockwise
			if (isP1) angle -= aimSpeed * Time.deltaTime;
			// counterclockwise
			else angle += aimSpeed * Time.deltaTime;

			// constrain to aiming range
			if (angle < minAngle) angle = minAngle;
			else if (angle > minAngle + aimRange) angle = minAngle + aimRange;
		}

		// position & rotation of the cursor on fRadius
		gunPos = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right * fRadius);
		gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		// apply the cursor's new position with an added offset for minor tweaking
		gun.position = transform.position + gunPos + gunPosOffset;

		// charge shots?
		// if(Input.GetKeyUp(KeyCode.F)) shotCharge = 0;

		//fire handling
		if(fireKey && canFire)
		{
			FireWeapon (10); //damage should depend on shot type
			canFire = false;
			StartCoroutine (FireRoutine (fireRate));
		}


		if(shopKey)
		{
			if(!shopOpen)
			{
				GameObject shop = Instantiate(shopPrefab, gameObject.transform.position + Vector3.up * 5, Quaternion.identity);
				shop.transform.SetParent(gameObject.transform);
				shopOpen = true;
			}else{
				//simply finding an arbitrary G.O. w/ a certain tag could lead to problems
				//this may be causing Issue#2
				Destroy(GameObject.FindGameObjectWithTag("EditorOnly"));
				shopOpen = false;
			}
		}
	}

	IEnumerator FireRoutine(float duration)
	{
		yield return new WaitForSeconds (duration);
		canFire = true;
	}

	private void FireWeapon(int damage)
	{
		GameObject curBullet = Instantiate (bullet, 
			gun.transform.position + (gun.transform.right * bulletSpawnOffset), 
			gun.transform.rotation);
		//curBullet = damage;
		//curBullet.GetComponent<Bullet>().
		Rigidbody2D rb = curBullet.GetComponent<Rigidbody2D> ();
		rb.AddForce(new Vector2(gun.transform.right.x, gun.transform.right.y) * bulletForce);
	}
		
	//shouldnt be checked every frame? need a better way of handling health
	//should only check when someone takes damage
	//really the handling of the healthbar should be in its own script attached to 
	private void UpdateHealth()
	{

		//get rid of coroutine for winscreen. that was only for the presentation
		if(currentHealth <= 0)
		{
			StartCoroutine(WinScreenRoutine(5.0f));
			currentHealth = maxHealth;
			opponent.GetComponent<PlayerController>().currentHealth = maxHealth;
		}

		healthBar.value = currentHealth;
	}

	IEnumerator WinScreenRoutine(float duration)
	{
		//print winscreen message
		//healthBar.GetComponent<Slider>().Value = currentHealth;
		Text txt = canvas.GetComponentInChildren<Text>();
		txt.enabled = true;
		if(isP1)
		{
			txt.text = "Player 2 Wins";
		}
		else
		{
			txt.text = "Player 1 Wins";
		}
		yield return new WaitForSeconds(duration);
		txt.enabled = false;
	}
}
