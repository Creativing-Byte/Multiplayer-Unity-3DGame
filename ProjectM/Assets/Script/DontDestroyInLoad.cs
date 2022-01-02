using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyInLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
