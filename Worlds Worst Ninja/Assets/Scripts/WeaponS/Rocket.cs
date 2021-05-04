using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Arrow arrow;
    private WeaponStat _WS;
    private Vector2 _dir;
    public float Speed,Lifetime;
    public GameObject Explosion,Particles;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        arrow = FindObjectOfType<Arrow>();
        _dir = arrow.dir;
        animator = GetComponent<Animator>();
        _WS = FindObjectOfType<WeaponStat>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(_dir.x, _dir.y,0)*Speed;
        Lifetime -= Time.deltaTime;
        if(Lifetime<=0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==8|| collision.gameObject.layer == 9 || collision.gameObject.layer == 12)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Instantiate(Particles, transform.position, transform.rotation);
            transform.position = transform.position;
            
                Destroy(this.gameObject);
            

        }
    }
    
}
