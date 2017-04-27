using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBullet : MonoBehaviour {
	public Bullet bull;
	public GameObject splitBullet;

	// Use this for initialization
	void Start () {
		bull = gameObject.GetComponent<Bullet> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool fire;

		if(bull.parentPlayer.isP1)
			fire = Input.GetKeyDown (KeyCode.F);
		else
			fire = Input.GetKeyDown (KeyCode.Semicolon);
		
		if (fire) 
		{
			//GameObject curBullet = Instantiate (splitBullet, transform, transform.rotation);
			GameObject curBullet1 = Instantiate (splitBullet, transform);
			GameObject curBullet2 = Instantiate (splitBullet, transform);
			Rigidbody2D tmp;
			tmp = curBullet1.GetComponent<Rigidbody2D> ();
			tmp.AddForce (tmp.velocity + bull.spread * Vector2.right);
			tmp = curBullet2.GetComponent<Rigidbody2D> ();
			tmp.AddForce (tmp.velocity + bull.spread * Vector2.left);

			//curBullet2.GetComponent<Bullet>().GetFiringPlayer(this);

		}
	}
}
