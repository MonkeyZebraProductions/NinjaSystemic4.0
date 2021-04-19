using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWall : MonoBehaviour
{

    private PlayerMovement _pm;
    public bool IsLeftWall;
    private bool _hasLeftLeft,_hasLeftRight;
    // Start is called before the first frame update

    private void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        
        
        if(_hasLeftLeft && !_pm._isJumping)
        {
            _pm.IsLeftWalled = false;
            _hasLeftLeft = false;
        }
        if (_hasLeftRight && !_pm._isJumping)
        {
            _pm.IsRightWalled = false;
            _hasLeftRight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            if (IsLeftWall)
            {
                _pm.IsLeftWalled = true;
            }
            else
            {
                _pm.IsRightWalled = true;
            }
            if(_pm._isJumping==true)
            {
                _pm._wallJumpTime = 0;
            }
            else
            {
                _pm._wallJumpTime = _pm.WallJumpTimer;
            }
            
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (IsLeftWall)
            {
                _pm.IsLeftWalled = true;
            }
            else
            {
                _pm.IsRightWalled = true;
            }
            if (_pm._isJumping == true)
            {
               // _pm._wallJumpTime = 0;
            }
            else
            {
                _pm._wallJumpTime = _pm.WallJumpTimer;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer==9)
        {
            if (IsLeftWall)
            {
                _hasLeftLeft = true;
            }
            else
            {
                _hasLeftRight = true;
            }
        }
        
    }
}
