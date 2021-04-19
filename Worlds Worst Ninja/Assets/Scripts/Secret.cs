using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret : MonoBehaviour
{

    public GameObject Seek;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        Seek.SetActive(true); 
    }
}
