using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzamiento : MonoBehaviour
{
    public GameObject throwableTriggerPosition;
    // Start is called before the first frame update
    void Start()
    {
       
    }

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
