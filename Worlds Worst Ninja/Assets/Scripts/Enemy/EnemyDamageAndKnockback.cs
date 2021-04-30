using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageAndKnockback : MonoBehaviour
{
    public bool hasShield = false;

    public float Health=10f;

    public HealthBar healthBar;

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

        healthBar.SetMaxHealth(Health);
    }

    private void Update()
    {
        _WS = FindObjectOfType<WeaponStat>();
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    public void HitEnemy()
    {
        Debug.Log("yo");
        _rb.AddForce(arrow.dir * 1000f * _WS.WeaponForce);
        if (GetComponent<EnemyAI>().playerSeen == false)
        {
            if(!hasShield)
                Health -= _WS.WeaponDamage;
            else
                Health -= _WS.WeaponDamage / 2;
        }
        else
            Health -= _WS.WeaponDamage / damageReductionValue;

<<<<<<< Updated upstream
        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
=======
        
>>>>>>> Stashed changes

        Debug.Log("Hi");
    }

    public void FirendlyFire(int damage)
    {
        if(!hasShield)
            Health -= damage;
        else
            Health -= damage / 2;

        healthBar.SetHealth(Health);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

<<<<<<< Updated upstream
=======


>>>>>>> Stashed changes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            if (!hasShield)
            {
                Health -= ExplosionDamage;
            }
            else
            {
                Health -= ExplosionDamage / 2;
            }

            ExplosionDirection = (collision.gameObject.transform.position - transform.position);
            ExplosionDirection.Normalize();
            _rb.AddForce(ExplosionDirection * 1000f);

            healthBar.SetHealth(Health);
        }
        if (collision.gameObject.layer == 16)
        {
            HitEnemy();
        }
    }
}
