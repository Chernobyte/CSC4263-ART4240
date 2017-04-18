using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    /*public float knockbackStrength = 2.0f;
    public Vector2 knockbackDirection;*/
    public int damage = 10;
    //public float stunTime = 1.0f;
    public float bulletForce = 1500.0f;
    public float lifespan = 5.0f;
    public GameObject hurtboxTriggerObject;

	private bool isFired;
    private float spawnTime;
    private Vector2 oldVelocity;
    private Rigidbody2D _rigidBody;
    private PlayerController parentPlayer;
    private Vector2 fireDirection;

	private void Start()
    {
        spawnTime = Time.time;
        _rigidBody = GetComponent<Rigidbody2D>();
		isFired = false;

        var hurtboxTrigger = hurtboxTriggerObject.GetComponent<TriggerCallback>();
        hurtboxTrigger.Init(OnHurtboxTriggerEnter2D, OnHurtboxTriggerExit2D, null);
    }

	private void Update()
    {
		if (!isFired) 
		{
			_rigidBody.AddForce (new Vector2 (transform.right.x, transform.right.y) * bulletForce);
			isFired = true;
		}
		
        if (Time.time - spawnTime > lifespan)
        {
            Destroy(gameObject);
        }
    }

    /*public void Initialize(Vector2 direction, PlayerController player)
    {
        parentPlayer = player;

        fireDirection = direction.normalized;
    }*/

	//replace with Initialize that also gets other 
	public void GetFiringPlayer(PlayerController player) 
	{
		parentPlayer = player;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		//var contact = collision.contacts;
		PlayerController hitPlayer = collision.gameObject.GetComponent<PlayerController>();
		if (hitPlayer != parentPlayer)
			Destroy (gameObject);
		//Destroy (gameObject);

		//add audio here

		//destroy terrain??? That ain't my job
    }

    private void OnHurtboxTriggerEnter2D(Collider2D collider)
    {
        var hitbox = collider.gameObject.GetComponent<HitboxCallback>();

        if (hitbox != null)
        {
            var player = hitbox.parentPlayer;

            if (player != parentPlayer)
            {
                /*Vector2 trueKnockbackDirection;

                if (_rigidBody.velocity.x > 0)
                {
                    trueKnockbackDirection = new Vector2(knockbackDirection.x, knockbackDirection.y);
                }
                else
                {
                    trueKnockbackDirection = new Vector2(-knockbackDirection.x, knockbackDirection.y);
                }

                trueKnockbackDirection.Normalize();

                player.TakeHit(trueKnockbackDirection * knockbackStrength, damage, stunTime);
                Destroy(gameObject);*/
				player.TakeDmg (damage);
				//playerPlayer.AddCurrency
            }
        }
    }

    private void OnHurtboxTriggerExit2D(Collider2D collider)
    {

    }
}
