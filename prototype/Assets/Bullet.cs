using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	GameObject platform;

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
			Destroy (gameObject);
		}
	}
}
