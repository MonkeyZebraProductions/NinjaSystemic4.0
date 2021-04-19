using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    // Start is called before the first frame update

    public float Duration = 5f;
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Destroy()
    {
        // Play explosion animation

        yield return new WaitForSeconds(Duration);

        Destroy(gameObject);
    }
}
