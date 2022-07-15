using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionPlayer : MonoBehaviour
{
    float Aspect;
    float Rounded;
    CanvasScaler cv;
#pragma warning disable CS0108 // El miembro oculta el miembro heredado. Falta una contraseña nueva
    public Camera camera;
#pragma warning restore CS0108 // El miembro oculta el miembro heredado. Falta una contraseña nueva
    void Awake()
    {
        cv = GetComponent<CanvasScaler>();
    }
    void Update()
    {
        if (cv == null)
        {
            cv = GetComponent<CanvasScaler>();
        }

        if (camera != null)
        {
            Rounded = (int)(Aspect * 100f) / 100f;

            if (Rounded == 0.6f || Rounded == 0.56f || Rounded == 0.48f || Rounded == 0.44f || Rounded == 0.75f || Rounded == 0.42f)
            {
                Addratios(0);
            }
            else if (Rounded == 0.54f || Rounded == 0.55f)
            {
                Addratios(1);
            }
        }
    }
    void Addratios(float m)
    {
        if (cv != null)
        {
            cv.matchWidthOrHeight = m;
        }
    }
}
