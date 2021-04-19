using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShooting : MonoBehaviour
{
    // Distance required to start attacking the player
    public float distanceRequired = 5f;

    private bool canShoot = true;

    [Header("Number of seconds between each shot")]
    public float fireRate = 1f;
    public int enemyDamage = 10;
    public GameObject bulletPrefab;
    public Transform firePoint;

    //public Text error;

    [Header("For testing only")]
    public bool forcedShoot = false;

    private float baseSpeed;

    private void Start()
    {
        //distanceRequired = GetComponent<EnemyVision>().visionDistance;

        baseSpeed = GetComponent<EnemyAI>().speed;
    }

    private void Update()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        //error.text = distanceToPlayer.ToString();

        if(distanceToPlayer <= distanceRequired)
        {
            if (GetComponent<EnemyAI>().playerSeen == true && canShoot)
            {
                GetComponent<EnemyAI>().canLookAround = false;
                GetComponent<EnemyAI>().speed = 0f;
                StartCoroutine(Shoot());
            }
        }
        else
        {
            GetComponent<EnemyAI>().speed = baseSpeed;
        }

        // For testing only
        if(forcedShoot && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if(GetComponent<EnemyAI>().goingRight == false)
        {
            bullet.GetComponent<EnemyBullet>().speed *= -1f;
        }

        yield return new WaitForSeconds(fireRate);

        canShoot = true;
    }
}
