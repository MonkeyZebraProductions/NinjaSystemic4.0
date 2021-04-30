using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{

    private Controls inputs;

    [SerializeField] public float MovementSpeed = 5f;
    [SerializeField] public float JumpSpeed = 5f;
    [SerializeField] public float HangTimer = 0.1f;
    [SerializeField] public float DefaultForce = 1000f;
    [SerializeField] public int MaxOverheat = 1000;
    [SerializeField] public float ExplosionForce = 10f;
    [SerializeField] public int MaxJumps = 1;
    public float Health = 10;

    public float WallJumpTimer, _reloadTime=1;

    public GameObject SelectedWeapon;

    public Transform LootTarget,LeftWallPoint,RightWallPoint;

    public Camera camera;

    private Vector2 move,look;

    private bool _isGrounded,_canMove,_firingSingle, _isRightWalled, _isLeftWalled,_canRightWall,_canLeftWall,_firstGrab,_isWallJumping;

    public bool  IsVisable, _canFire, _isAuto, _isFiring,_isExplosion, _isJumping, _hasSwitched;

    private float _jumpMultiplyer, _airMultiplier, ExplosionMultiplier = 1;

    private float _jumpMultiplyerRate, _ExplosionMultiplierRate, _hangTime, _wallJumpTime;

    private int _jumps,_overheatStep,_overHeat;

    private Rigidbody2D _rb2D;

    private Arrow arrow;

    public Vector2 ExplosionDirection;

    private PhysicalExplosion Expl;
    private ReloadVisual _RV;

    public SpriteRenderer sprite;
    private Color colour;

    private WeaponStat _WS;

    public LayerMask WhatIsWall;

    public Slider HealthSlider;

    void Awake()
    {
        inputs = new Controls();
        
        inputs.Player.Jump.started += context => Jump();
        inputs.Player.Jump.canceled += context => JumpCancel();
        inputs.Player.Fire.started += context => Fire();
        inputs.Player.Fire.canceled += context => FireCancel();
        inputs.Player.SwitchLeft.started += context => SwitchLeft();
        inputs.Player.SwichRight.started += context => SwitchRight();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        
        arrow = FindObjectOfType<Arrow>();
        
        Expl = FindObjectOfType<PhysicalExplosion>();

        _RV = FindObjectOfType<ReloadVisual>();

        _jumps = MaxJumps;
        _overheatStep = 1;
        _overHeat = 0;
        _canFire = false;
        _isFiring = false;
        colour = sprite.color;
        _WS = FindObjectOfType<WeaponStat>();
        _reloadTime = _WS.ReloadTime;
        _hangTime = HangTimer;
        _ExplosionMultiplierRate = 0.9f;
        _wallJumpTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrounded)
        {
            _jumps = MaxJumps;
            _firstGrab = false;
        }
        _WS = FindObjectOfType<WeaponStat>();
        _isAuto = _WS.IsAuto;
        
        if(Health<=0)
        {
            Destroy(gameObject);
        }
        _isLeftWalled=Physics2D.OverlapCircle(LeftWallPoint.position, 0.2f, WhatIsWall);
        _isRightWalled = Physics2D.OverlapCircle(RightWallPoint.position, 0.2f, WhatIsWall);
        
        Debug.Log(_canMove);
        if(_canRightWall && move.x>0 && !_isGrounded)
        {
            _isRightWalled= true;
        }
        
        if(_canLeftWall && move.x < 0 && !_isGrounded)
        {
            _isLeftWalled = true;
        }

        HealthSlider.value = Health;
    }

    private void FixedUpdate()
    {
        if (_wallJumpTime <= 0)
        {


            move = inputs.Player.Move.ReadValue<Vector2>();
            Vector3 mousePosition = inputs.Player.Look.ReadValue<Vector2>();
            //LootTarget.transform.position = new Vector3(look.x, look.y, 0);


            mousePosition.z = 20;
            mousePosition = camera.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            LootTarget.position = mousePosition;

            if (_rb2D.velocity.y <= 0 && !_firingSingle && !_isGrounded && !_isLeftWalled && !_isRightWalled)
            {
                Debug.Log("Yoo");
                _rb2D.AddForce(Vector2.up * -9.81f * JumpSpeed * 100f * Time.deltaTime * _airMultiplier);
                _airMultiplier += 0.075f;
            }

            //SelectedWeapon.transform.LookAt(LootTarget.transform, Vector2.up);
            
                _rb2D.velocity = new Vector2(move.x * MovementSpeed, 0);
            

            //checks if jump button was pressed
            if (_isJumping == true)
            {


                _rb2D.velocity += Vector2.up * JumpSpeed * _jumpMultiplyer;

                _jumpMultiplyer *= _jumpMultiplyerRate;

                if (_jumpMultiplyer <= 0.1f)
                {
                    _isJumping = false;
                }
            }

            if (_isLeftWalled || _isRightWalled)
            {
                if (!_firstGrab)
                {
                    _rb2D.velocity = new Vector2(0, 0);
                    _jumps += 1;
                    _firstGrab = true;
                }

                _airMultiplier = 1;
            }
        }
        else
        {
            if(_isLeftWalled)
            {
                _rb2D.velocity += new Vector2(MovementSpeed, JumpSpeed)*2;
                _isLeftWalled = false;
            }
            if (_isRightWalled)
            {
                _rb2D.velocity += new Vector2(-MovementSpeed, JumpSpeed)*2;
                _isRightWalled = false;
            }
            _wallJumpTime -= Time.deltaTime;
        }
       
        //Checks if holding fire button
        if (_canFire==true && _isAuto == true)
        {
            if(_overHeat<MaxOverheat)
            {
                _rb2D.AddForce(new Vector2(arrow.dir.x, arrow.dir.y) * DefaultForce * _WS.WeaponForce * -1f);
                _rb2D.AddForce(Vector2.up * -9.81f * JumpSpeed * 55f * Time.deltaTime);
                arrow.CreateDebris();
                //_WS.WeaponSound.Play();
            }
            else
            {
                _WS.WeaponSound.Stop();
            }
            _overHeat += _overheatStep;
            if(_overHeat>MaxOverheat)
            {
                _overHeat = MaxOverheat;
            }
        }
        else if(_overHeat>0)
        {
            _overHeat -= _overheatStep * 2;
            
        }
        //else
        //{
        //    _WS.WeaponSound.Stop();
        //}
        if(_firingSingle)
        {
            _rb2D.AddForce(new Vector2(arrow.dir.x, arrow.dir.y) * _WS.WeaponForce * DefaultForce * -1f);
            
            _hangTime -= Time.deltaTime;
            _airMultiplier = 1f;
            if (_hangTime < 0)
            {
                
                _firingSingle = false;
            }
        }

        if(_isExplosion==true)
        {
            _rb2D.AddForce(new Vector2(ExplosionDirection.x, ExplosionDirection.y) * ExplosionForce * DefaultForce * -1f*ExplosionMultiplier);
            ExplosionMultiplier *= _ExplosionMultiplierRate;
            if(ExplosionMultiplier<=0.1f)
            {
                _isExplosion = false;
            }
        }
        
        Reload();
        
    }

    private void Jump()
    {
        if (_isLeftWalled || _isRightWalled)
        {
            _wallJumpTime = WallJumpTimer;
            _jumps += 1;

        }
        if (_jumps>0)
        {
            
            
                _isJumping = true;
            
            
            _jumpMultiplyer = 1f;
            _jumpMultiplyerRate = 0.9f;
            _jumps -= 1;           
        }

        FindObjectOfType<AudioManager>().Play("Jump");
       
    }


    //Checks if Jump button is let go
    private void JumpCancel()
    {
        _jumpMultiplyerRate = 0.5f;
    }

    private void Fire()
    {
        
        _isFiring = true;
        if(_reloadTime>= _WS.ReloadTime)
        {
            _canFire = true;
            _WS.WeaponSound.Play();

        }
        if (_canFire == true && _isAuto == false)
        {
            
            _hangTime = HangTimer;
            _firingSingle = true;
            arrow.CreateDebris();
            
            _canFire = false;
            _RV.Rest();
            _reloadTime = 0;
            
        }
        else if(_isFiring)
        {
            _canFire = true;
            //_WS.WeaponSound.Play();
        }


    }

    
    //Checks if Fire button is let go
    private void FireCancel()
    {
        _canFire = false;
        _isFiring = false;
        if(_isAuto)
        {
            _WS.WeaponSound.Stop();
        }
        
    }

    void Reload()
    {
        if(_reloadTime<_WS.ReloadTime)
        {
            if(_hasSwitched)
            {
                _RV.ReSwitch();
                _reloadTime = _WS.ReloadTime*0.8f;
            }
            _reloadTime += Time.deltaTime;
        }
    }
    
    void SwitchLeft()
    {
        StartCoroutine(Switch());
    }

    void SwitchRight()
    {
        StartCoroutine(Switch());
    }

    public IEnumerator Switch()
    {
        _hasSwitched = true;
        yield return new WaitForSeconds(0.1f);
        _hasSwitched = false;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = true;
            _jumpMultiplyer=_airMultiplier = 1f;
        }
        if (collider2D.gameObject.layer == 10)
        {
            IsVisable = true;
            colour = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            sprite.color = colour;
        }
        if (collider2D.gameObject.layer == 13)
        {
            Debug.Log("Hai");
            ExplosionDirection = (collider2D.gameObject.transform.position - transform.position);
            ExplosionDirection.Normalize();
            ExplosionMultiplier = 1;
            _isExplosion = true;
        }
    }

    

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = true;
            _jumpMultiplyer= _airMultiplier = 1f;
        }
        if (collider2D.gameObject.layer == 10)
        {
            IsVisable = true;
            colour = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
            sprite.color = colour;
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 8)
        {
            _isGrounded = false;
            if(_isJumping==false)
            {
                _jumps -= 1;
            }
            
        }
        if (collider2D.gameObject.layer == 10)
        {
            IsVisable = false;
            colour = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
            sprite.color = colour;
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
