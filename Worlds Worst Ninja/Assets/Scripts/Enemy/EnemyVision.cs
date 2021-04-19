using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public bool useRaycast = false;

    [Header("For raycast only")]
    public LayerMask playerLayer;
    public float visionDistance = 5f;

    private RaycastHit2D raycastHit;
    private Color rayColor;

    private SpriteRenderer sprite;
    private Color defaultColor;
    public Color redColor;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        defaultColor = sprite.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!useRaycast)
        {
            if (collision.tag == "Player")
            {
                GetComponentInParent<EnemyAI>().targetPos = collision.transform;
                GetComponentInParent<EnemyAI>().playerSeen = true;

                sprite.color = redColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!useRaycast)
        {
            if(collision.tag == "Player")
            {
                sprite.color = defaultColor;
                GetComponentInParent<EnemyAI>().playerSeen = false;
            }
        }
    }

    private void Update()
    {
        if (useRaycast)
        {
            GetComponent<Collider2D>().enabled = false;

            Vector3 raycastOffset = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z);

            if (GetComponentInParent<EnemyAI>().goingRight == true)
            {
                raycastHit = Physics2D.Raycast(raycastOffset, Vector3.right, visionDistance, playerLayer);
                Debug.DrawRay(raycastOffset, Vector3.right * visionDistance, rayColor);
            }
            else
            {
                raycastHit = Physics2D.Raycast(raycastOffset, -Vector3.right, visionDistance, playerLayer);
                Debug.DrawRay(raycastOffset, -Vector3.right * visionDistance, rayColor);
            }

            if (raycastHit.collider != null)
            {
                rayColor = Color.green;
                GetComponentInParent<EnemyAI>().targetPos = raycastHit.collider.transform;

                GetComponentInParent<EnemyAI>().playerSeen = true;

                sprite.color = redColor;
            }
            else
            {
                rayColor = Color.red;

                GetComponentInParent<EnemyAI>().playerSeen = false;

                sprite.color = defaultColor;
            }
        }
    }
}
