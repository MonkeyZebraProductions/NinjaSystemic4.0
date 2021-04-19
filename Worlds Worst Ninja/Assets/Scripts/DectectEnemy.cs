using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DectectEnemy : MonoBehaviour
{
    public bool HasHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==12)
        {
            HasHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            HasHit = false;
        }
    }
}
