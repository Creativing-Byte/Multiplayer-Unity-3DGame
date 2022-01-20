using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FairyDeathDestroy : MonoBehaviour
{
    public PhotonView miView;
    public float time;
    private void Start()
    {
        miView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time>=10)
        {
            PhotonNetwork.Destroy(miView);
        }
    }
}
