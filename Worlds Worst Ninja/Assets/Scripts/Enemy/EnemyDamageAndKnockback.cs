using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageAndKnockback : MonoBehaviour
{
    public float Health=10f;

    public float damageReductionValue = 2f;

    public float ExplosionDamage=9f;

    public bool IsFront, IsBack;
    private Rigidbody2D _rb;
    private Arrow arrow;
    private WeaponStat _WS;
    private Vector2 ExplosionDirection;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        arrow = FindObjectOfType<Arrow>();
       
    }

    private void Update()
    {
        _WS = FindObjectOfType<WeaponStat>();
    }
    // Update is called once per frame
    public void HitEnemy()
    {

        _rb.AddForce(arrow.dir * 100f * _WS.WeaponForce);
        if (GetComponent<EnemyAI>().playerSeen == false)
            Health -= _WS.WeaponDamage;
        else
            Health -= _WS.WeaponDamage / damageReductionValue;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        Debug.Log("Hi");
    }

    public void FirendlyFire(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==13)
        {
            Health -= ExplosionDamage;
            ExplosionDirection = (collision.gameObject.transform.position - transform.position);
            ExplosionDirection.Normalize();
            _rb.AddForce(ExplosionDirection * 1000f);
        }
    }
}
