using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 5;
    private Rigidbody2D rb;
    private PlayerMovement _pm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _pm = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyDamageAndKnockback>().FirendlyFire(damage);
            Destroy(gameObject);
        }

        if(collision.tag == "Player")
        {
            _pm.Health -= damage;
            Destroy(gameObject);
        }
    }
}
