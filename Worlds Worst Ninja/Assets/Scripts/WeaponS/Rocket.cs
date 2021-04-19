using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Arrow arrow;
    private Vector2 _dir;
    public float Speed;
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        arrow = FindObjectOfType<Arrow>();
        _dir = arrow.dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(_dir.x, _dir.y,0)*Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer==8|| collision.gameObject.layer == 9 || collision.gameObject.layer == 12)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
