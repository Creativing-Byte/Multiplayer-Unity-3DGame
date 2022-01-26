using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotacion : MonoBehaviour
{
    public float rotacione;
    float x;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime * 10;
        transform.rotation = Quaternion.Euler(0, 0, x);
        rotacione = transform.rotation.z*180;
        if (rotacione==0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        print(rotacione);
    }
}
