using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Controls inputs;

    public Transform LookTarget;

    private float maxDirX, maxDirY, maxRadius,assaultTime;

    public float angle,AssaultTimer;

    public LayerMask WhatIsGround, WhatIsEnemy,WhatIsFront,WhatIsBack, WhatIsWall;

    public Vector2 dir, point;

    public bool _hitFront,_hitBack;

    private bool _hitground, _hitenemy, _hitwall;

    public GameObject Sound, BurstPart;

    public Transform Spawner;

    public RaycastHit2D hitGround, hitEnemy, hitWall;


    private PlayerMovement _pm;

    private WeaponStat _WS;

    private EnemyDamageAndKnockback _EDK;

    

    private LineRenderer line;

    private void Awake()
    {
        inputs = new Controls();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
        line = GetComponent<LineRenderer>();
        
    }

 


    // Update is called once per frame
    void FixedUpdate()
    {
        _WS = FindObjectOfType<WeaponStat>();
       
        if(_hitenemy)
        {
            maxRadius=Vector2.Distance(transform.position,hitEnemy.point);
        }
        else
        {
            maxRadius = _WS.WeaponRange;
        }
       
        Sound = _WS.Sound;
        
        BurstPart = _WS.BurstPart;
        
        Vector2 mousePosition = inputs.Player.Look.ReadValue<Vector2>();


        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        dir = (mousePosition - pos);
        dir.Normalize();



        angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        

        //_hitground = hitGround = Physics2D.Raycast(transform.position, dir, maxRadius, WhatIsGround);

        //_hitwall = hitWall = Physics2D.Raycast(transform.position, dir, maxRadius, WhatIsWall);

       
        line.SetPosition(1, new Vector3(maxRadius, 0, 0));


        if(assaultTime<AssaultTimer)
        {
            assaultTime += Time.deltaTime;
        }
    }


    public void CreateDebris()
    {

        Instantiate(Sound, transform.position, Quaternion.identity);
        Instantiate(BurstPart, transform.position, Quaternion.identity);
        if(_WS.IsAuto)
        {
            if(assaultTime>=AssaultTimer)
            {
                Instantiate(_WS.Rocket, Spawner.position, Quaternion.Euler(0, 0, angle));
                assaultTime = 0;
            }
        }
        else
        {
            Instantiate(_WS.Rocket, Spawner.position, Quaternion.Euler(0, 0, angle));
        }
        
    }

    

   



    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
