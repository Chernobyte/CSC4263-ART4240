using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	GameObject platform;
	//public GameObject player;

	// Use this for initialization
	void Start () {
		platform = GameObject.FindWithTag ("Platform");

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 diff = transform.position - platform.transform.position;

		if (diff.magnitude > 50f)
			Destroy (gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag ("Platform") || collision.gameObject.CompareTag ("Player")) 
		{
			if(collision.gameObject.name != "PLBody")
			{
				if(collision.gameObject.name == "PRBody")
					collision.gameObject.GetComponent<PlayerController>().currentHealth -= 10;
				Destroy (gameObject);
			}
		}
	}
}
