using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    public bool alwaysShowHearingRange = true;

    public float hearingRange = 10f;

    private void Start()
    {
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = hearingRange;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ExplosionSound")
        {
            Transform targetPos = collision.transform;
            GetComponentInParent<EnemyAI>().StoreTargetPos(targetPos);

            GetComponentInParent<EnemyEmotions>().SoundHeard();
        }
    }

    private void OnDrawGizmos()
    {
        if (alwaysShowHearingRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, hearingRange);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}
