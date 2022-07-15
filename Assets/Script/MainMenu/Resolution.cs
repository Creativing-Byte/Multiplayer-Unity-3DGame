using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour
{
    float Aspect;
    float Rounded, PM;
    CanvasScaler cv;
    Camera camara;
    public bool UIBattle, Tutorial;
    PhotonView pv;
    void Awake()
    {
        cv = GetComponent<CanvasScaler>();
        if (UIBattle)
        {
            pv = GetComponent<PhotonView>();
        }
    }
    void Update()
    {
        if (UIBattle && !Tutorial)
        {
            if (pv.IsMine)
            {
                if (camara == null)
                {
                    camara = GetComponentInParent<Camera>();
                }
                Aspect = camara.aspect;
            }
        }
        else if (UIBattle && Tutorial)
        {
            if (camara == null)
            {
                camara = GetComponentInParent<Camera>();
            }
            Aspect = camara.aspect;
        }
        else
        {
            try
            {
                if (camara == null)
                {
                    camara = Camera.main;
                }
                Aspect = camara.aspect;
            }
            catch (NullReferenceException)
            {
            }
        }

        if (camara != null)
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
