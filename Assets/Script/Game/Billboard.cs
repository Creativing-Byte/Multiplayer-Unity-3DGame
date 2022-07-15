using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform cam;

    private void Awake()
    {
        var p1 = GameObject.Find("P1");
        var p2 = GameObject.Find("P2");
     
        if (p1 != null) cam = p1.transform.parent;
        if (p2 != null) cam = p2.transform.parent;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
