using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLanzables : MonoBehaviour
{
    public Rigidbody rb;

    [Range(-50, 0)] public float velocidad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rb.velocity = new Vector3(0, velocidad, 0);
    }
}
