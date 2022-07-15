using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class Regioninfo : MonoBehaviour
{
    public Text Text;

    string _cache;

    void Update()
    {
        if (PhotonNetwork.CloudRegion != _cache)
        {
            _cache = PhotonNetwork.CloudRegion;
            if (string.IsNullOrEmpty(_cache))
            {
                Text.text = "n/a";
            }
            else
            {
                Text.text = _cache;
            }
        }
    }
}
