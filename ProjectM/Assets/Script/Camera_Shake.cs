using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System;
using UnityEngine;

public class Camera_Shake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float Duration = 1f;

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }

    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / Duration);
            transform.position = startPosition + UnityEngine.Random.insideUnitSphere * strength;
            yield return null;


        }
        transform.position = startPosition;
    }



}
