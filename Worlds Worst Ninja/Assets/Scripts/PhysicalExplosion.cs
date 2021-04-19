using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalExplosion : MonoBehaviour
{

    public float force;

    public float FOI;

    public LayerMask hitLayer;

    private AudioSource _as;

    private PlayerMovement _pm;
    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _as.Play();
        Destroy(this.gameObject, 0.2f);
        _pm = FindObjectOfType<PlayerMovement>();
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOI);
    }
}
