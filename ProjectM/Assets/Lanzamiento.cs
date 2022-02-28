using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzamiento : MonoBehaviour
{
    public GameObject throwableTriggerPosition;

    // Update is called once per frame
    void Update()
    {
        print("1");
        if (throwableTriggerPosition == null)
        {
            throwableTriggerPosition = GameObject.Find("lanzamientothrowable");
            print("2");
        }

    }
}
