﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	//
	// Will restructure code later. It's pretty ugly and inefficient at the moment
	//
	public bool isP1;
	public float aimSpeed = 100.0f;
	public float maxSpeed = 10.0f;
	public int maxHealth = 100;
	public int currentHealth;

	//for aiming
	public Transform gun;
	public float fRadius = 1.0f;
	public float bulletForce = 5.0f;
	public Vector3 gunPosOffset = new Vector3(0.0f, 0.0f, -0.1f); //use this to line up arm with character's shoulder
	//bullet
	public GameObject bullet;
	public float bulletSpawnOffset = 1.2f;
	public float fireRate = 1.0f;

	private float angle;
	private Vector3 gunPos;
	private bool canFire = true;
	private bool shopOpen = false;

	//private Overlord overlord;
	//private PlayerUI playerUI;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;

	public GameObject opponent;
	public GameObject shopPrefab;
	public Canvas canvas;

	private float currentSpeed = 0.0f;
	private int shotCharge = 0;

	// Use this for initialization
	void Start () 
	{
		_rb = gameObject.GetComponent<Rigidbody2D>();
		//_col = gameObject.GetComponent<BoxCollider2D>();

		currentHealth = maxHealth;

		if(isP1)
		{
			angle = 0.0f;
			gunPos = new Vector3 (fRadius, 0.0f, 0.0f);
		}
		else
		{
			angle = 180.0f;
			gunPos = new Vector3 (-fRadius, 0.0f, 0.0f);
		}
		gun.position = transform.position + gunPos + gunPosOffset;
	}
	
	// Update is called once per frame
	void Update () {
		CheckHealth();
		HandleInput();
	}

	private void HandleInput()
	{
		//handle movemement

		bool moveLeft, moveRight, aimUp, aimDown,fireKey;

		//Player 1
		if(isP1)
		{
			moveLeft 	= Input.GetKey(KeyCode.A);
			moveRight 	= Input.GetKey(KeyCode.D);
			aimUp 		= Input.GetKey(KeyCode.W);
			aimDown 	= Input.GetKey(KeyCode.S);
			fireKey		= Input.GetKey(KeyCode.F);
		}
		//Player 2
		else
		{
			moveLeft 	= Input.GetKey(KeyCode.LeftArrow);
			moveRight 	= Input.GetKey(KeyCode.RightArrow);
			aimUp 		= Input.GetKey(KeyCode.UpArrow);
			aimDown 	= Input.GetKey(KeyCode.DownArrow);
			fireKey		= Input.GetKey(KeyCode.Semicolon);
		}
			
		if(moveLeft)
		{
			//Vector3 movement = new Vector3(
			_rb.velocity = new Vector3(-maxSpeed, 0.0f, 0.0f);
		}
		else if(moveRight)
		{
			//Vector3 movement = new Vector3(
			_rb.velocity = new Vector3(maxSpeed, 0.0f, 0.0f);
		}


		// handle aiming 

		if (aimUp) 
		{
			if(isP1)
			{
				angle += aimSpeed * Time.deltaTime;
				if(angle > 60.0f)
					angle = 60.0f;
			}
			else
			{
				angle -= aimSpeed * Time.deltaTime;
				if(angle < 120.0f)
					angle = 120.0f;
			}
			gunPos = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right * fRadius);
			//gun.position = transform.position + gunPos + gunPosOffset;

			gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		else if(aimDown)
		{
			if(isP1)
			{
				angle -= aimSpeed * Time.deltaTime; 
				if(angle < 0.0f)
					angle = 0.0f;
			}
			else
			{
				angle += aimSpeed * Time.deltaTime; 
				if(angle > 180.0f)
					angle = 180.0f;
			}
			gunPos = Quaternion.AngleAxis(angle, Vector3.forward) * (Vector3.right * fRadius);
			//gun.position = transform.position + gunPos + gunPosOffset;

			gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		gun.position = transform.position + gunPos + gunPosOffset;

		//if(Input.GetKeyUp(KeyCode.F)) shotCharge = 0;

		if((( isP1 && (fireKey && Input.GetKey(KeyCode.F))) || 
			(!isP1 && (fireKey && Input.GetKey(KeyCode.Semicolon)))) && canFire)
		{
			FireWeapon (10);
			canFire = false;
			StartCoroutine (FireRoutine (fireRate));
		}

		//all of this is just for the demo. will implement properly later
		if((isP1 && Input.GetKeyDown(KeyCode.G)) || (!isP1 && Input.GetKeyDown(KeyCode.Quote)))
		{
			if(!shopOpen)
			{
				GameObject shop = Instantiate(shopPrefab, gameObject.transform.position + Vector3.up * 5, Quaternion.identity);
				shop.transform.SetParent(gameObject.transform);
				shopOpen = true;
			}else{
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
		
	private void CheckHealth()
	{
		if(currentHealth <= 0)
		{
			StartCoroutine(WinScreenRoutine(5.0f));
			currentHealth = maxHealth;
			opponent.GetComponent<PlayerController>().currentHealth = maxHealth;
		}
			
		Slider healthBar;

		if(isP1)
		{
			healthBar = GameObject.Find("P1 Healthbar").GetComponent<Slider>();
		}
		else
		{
			healthBar = GameObject.Find("P2 Healthbar").GetComponent<Slider>();
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
