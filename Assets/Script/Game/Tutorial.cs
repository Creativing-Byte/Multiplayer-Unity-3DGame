using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public bool Active;
    void Update()
    {
        if (Active)
        {
            Deactive();
            Active = false;
        }
    }
    public void Deactive()
    {
        this.enabled = false;
    }
}
