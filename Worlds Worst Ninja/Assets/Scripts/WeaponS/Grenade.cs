using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Arrow arrow;
    private Vector2 _dir;
    private Rigidbody2D _rb;
    public float Speed, GrenadeMultiplier, Countdown;
    
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        arrow = FindObjectOfType<Arrow>();
        _dir = arrow.dir;
    }

    // Update is called once per frame
    void Update()
    {
        _rb.AddForce(new Vector2(_dir.x, _dir.y) * Speed * GrenadeMultiplier);
        GrenadeMultiplier *= GrenadeMultiplier;
        Countdown -= Time.deltaTime;
        if(Countdown<=0)
        {
            Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Vector2 ExplosionDirection;
            ExplosionDirection = (collision.gameObject.transform.position - transform.position);
            ExplosionDirection.Normalize();
            _rb.AddForce(ExplosionDirection * 1000f);
        }
    }
}
