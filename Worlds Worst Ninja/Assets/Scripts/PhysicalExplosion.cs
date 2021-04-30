using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalExplosion : MonoBehaviour
{

    public float force;

    public float FOI;

    

    private AudioSource _as;

    
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _as.Play();
        Destroy(this.gameObject, 0.2f);
        
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOI);
    }
}
