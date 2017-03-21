using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    float explosionStart;

	void Start ()
    {
        explosionStart = Time.time;
	}
	
	void Update ()
    {
		if(Time.time - explosionStart >.2)
        {
            Destroy(gameObject);
        }
	}
}
