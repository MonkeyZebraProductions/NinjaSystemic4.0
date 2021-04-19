using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadVisual : MonoBehaviour
{

    private Animator animator;
    private WeaponStat _WS;
    
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _WS = FindObjectOfType<WeaponStat>();
        animator.speed = 1 / _WS.ReloadTime;
    }

    public void Rest()
    {
        animator.Play("Circle", -1, 0f);
    }

   public void ReSwitch()
   {
        animator.Play("Circle", -1, 0.8f);
    }
}
