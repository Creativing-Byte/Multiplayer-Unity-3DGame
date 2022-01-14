using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DestroyPunch : MonoBehaviour
{
    public PhotonView miView;
    private void Start()
    {
        miView = GetComponent<PhotonView>();
    }
    public void DestroyMiView()
    {
        PhotonNetwork.Destroy(miView);
    }
}
