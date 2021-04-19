using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    [Header("Values")]
    public float soundRadius = 5f;
    public float soundDuration = 3f;

    private void Start()
    {
        gameObject.tag = "ExplosionSound";

        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = soundRadius;
        collider.isTrigger = true;

        StartCoroutine(Destroy());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }

    IEnumerator Destroy()
    {
        // Play explosion animation

        yield return new WaitForSeconds(soundDuration);

        Destroy(gameObject);
    }
}
