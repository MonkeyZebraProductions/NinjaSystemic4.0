using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthKill : MonoBehaviour
{

    private Controls inputs;

    private GameObject enemy;

    private void Awake()
    {
        inputs = new Controls();
        inputs.Player.Attack.started += context => Attack();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemy);
    }

    private void Attack()
    {
        Destroy(enemy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==12)
        {
            enemy = collision.gameObject;
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
