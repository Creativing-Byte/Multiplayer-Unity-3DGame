using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public Tower canon;
    // Start is called before the first frame update
    void Start()
    {
        canon = GetComponentInParent<Tower>();
        canon.canonInst = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
