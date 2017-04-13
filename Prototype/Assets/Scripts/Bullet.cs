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

    //private int numBouncesRemaining = 0;
    private float spawnTime;
    private Vector2 oldVelocity;
    private Rigidbody2D _rigidBody;
    private PlayerController parentPlayer;
    private Vector2 fireDirection;

    //public Sprite bulletImg1;
    //public Sprite bulletImg2;

	private void Start()
    {
        spawnTime = Time.time;
        _rigidBody = GetComponent<Rigidbody2D>();
		_rigidBody.AddForce(new Vector2(transform.right.x, transform.right.y) * bulletForce);//.velocity = fireDirection * bulletSpeed;

        var hurtboxTrigger = hurtboxTriggerObject.GetComponent<TriggerCallback>();
        hurtboxTrigger.Init(OnHurtboxTriggerEnter2D, OnHurtboxTriggerExit2D, null);
    }

	private void Update()
    {
        if (Time.time - spawnTime > lifespan)
        {
            Destroy(gameObject);
        }
        /*if (Time.time / 1 == 0)
            gameObject.GetComponent<SpriteRenderer>().sprite = bulletImg2;
        if (Time.time /.5 == 0)
            gameObject.GetComponent<SpriteRenderer>().sprite = bulletImg1;*/
    }

    /*private void FixedUpdate()
    {
        oldVelocity = _rigidBody.velocity;
    }*/

    /*public void Initialize(Vector2 direction, PlayerController player)
    {
        parentPlayer = player;

        fireDirection = direction.normalized;

		_rigidBody.AddForce(fireDirection * bulletForce);
    }*/

	public void GetFiringPlayer(PlayerController player) 
	{
		parentPlayer = player;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		//var contact = collision.contacts;
		//Destroy (gameObject);
		PlayerController hitPlayer = collision.gameObject.GetComponent<PlayerController>();
		if (hitPlayer != parentPlayer)
			Destroy (gameObject);
		//Destroy (gameObject);

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
            }
        }
    }

    private void OnHurtboxTriggerExit2D(Collider2D collider)
    {

    }
}
