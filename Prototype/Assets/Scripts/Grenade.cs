using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    float pinPull;
    public float pinTimer =2;
    public GameObject explosion;
    GameObject collidedObj = null;
    bool col = false;

	void Start ()
    {
        pinPull = Time.time;
	}
	
	void Update ()
    {
        if (Time.time - pinPull > pinTimer)
        {
            GameObject explode = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(collidedObj!=null && collidedObj.GetComponent<Rigidbody2D>()!=null)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = collidedObj.GetComponent<Rigidbody2D>().velocity;
        }
        else if(collidedObj != null)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        
   	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collidedObj = collision.gameObject;
    }
}
