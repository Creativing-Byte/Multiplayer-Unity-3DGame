using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCMcontrol : MonoBehaviour
{
    public Camera_Shake BoolActivate1;
    public Camera_Shake BoolActivate2;
    void CameraDestroyShake()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Camera2");
        GameObject g2 = GameObject.FindGameObjectWithTag("Camera1");
        BoolActivate2 = g.GetComponent<Camera_Shake>();
        BoolActivate2.start = true;
        BoolActivate1 = g2.GetComponent<Camera_Shake>();
        BoolActivate1.start = true;
    }
}
