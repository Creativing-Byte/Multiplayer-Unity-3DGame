using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySonido : MonoBehaviour
{
    public SfxManager active;
    // Start is called before the first frame update
    void Start()
    {
        active = GameObject.Find("Launcher").GetComponent<SfxManager>();
        active.active = true;
    }

}
