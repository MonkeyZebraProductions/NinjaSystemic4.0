using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform targetPos;
    private Vector3 target;
    public float activateDist = 5f;
    public float pathUpdateSeconds = 1f;
    public LayerMask obstacleLayer;

    [Header("Physics")]
    public float speed = 3f;
    public float nextWaypointDist = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded = false;
    private Seeker seeker;
    private Rigidbody2D rb;

    // Checking for obstacles
    private bool obstacleInFront;
    private RaycastHit2D raycastHit;
    private float jumpDist = 1.5f;

    [Header("Giving up on the player")]
    public bool playerSeen = false;
    public float forgetPlayerTimer = 10f;
    private float timer;


    [Header("Looking around")]
    public float lookingAroundTimer = 3f;
    private bool lookingAround = false;

    [HideInInspector] public bool goingRight = false;
    [HideInInspector] public bool canLookAround = true;

    private void Start()
    {
        if(targetPos == null)
        {
            target = transform.position;
        }
        else
        {
            target = targetPos.position;
        }

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        timer = forgetPlayerTimer;
    }

    private void Update()
    {
        if (targetPos != null)
            target = targetPos.position;

        if (!playerSeen)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                canLookAround = true;

                targetPos = null;
                timer = forgetPlayerTimer;
            }
        }
        else
        {
            timer = forgetPlayerTimer;
        }

        // Checking if the enemy is standing still to look around

        if (rb.velocity.x <= 0.1f && !lookingAround)
        {
            if(canLookAround)
                StartCoroutine(LookAround());
        }

        if (playerSeen)
        {
            GetComponent<EnemyEmotions>().PlayerSeen();
        }
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            if (TargetInDistance() && followEnabled)
            {
                PathFollow();
            }
        }
    }

    IEnumerator LookAround()
    {
        lookingAround = true;

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        goingRight = false;

        yield return new WaitForSeconds(lookingAroundTimer);

        transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        goingRight = true;

        yield return new WaitForSeconds(lookingAroundTimer);

        lookingAround = false;
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // See if Grounded
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);


        // Directional Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Jump
        if(jumpEnabled && isGrounded)
        {
            if (obstacleInFront)
            {
                rb.AddForce(Vector2.up * jumpModifier);
            }
        }

        // Movement
        rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDist)
        {
            currentWaypoint++;
        }

        // Directional Graphics Handling and checking for obstacles
        Vector3 raycastOffset = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        if (directionLookEnabled)
        {
            Color rayColor;

            if (raycastHit.collider != null)
            {
                rayColor = Color.green;
                obstacleInFront = true;
            }
            else
            {
                rayColor = Color.red;
                obstacleInFront = false;
            }

            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                raycastHit = Physics2D.Raycast(raycastOffset, Vector3.right, jumpDist, obstacleLayer);
                Debug.DrawRay(raycastOffset, Vector3.right * jumpDist, rayColor);

                goingRight = true;
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                raycastHit = Physics2D.Raycast(raycastOffset, -Vector3.right, jumpDist, obstacleLayer);
                Debug.DrawRay(raycastOffset, -Vector3.right * jumpDist, rayColor);

                goingRight = false;
            }
        }

    }

    private bool TargetInDistance()
    {
        if (target != null)
            return Vector2.Distance(transform.position, target) < activateDist;
        else
            return false;
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void StoreTargetPos(Transform soundTarget)
    {
        target = new Vector3(soundTarget.position.x, soundTarget.position.y, soundTarget.position.z);
    }
}
