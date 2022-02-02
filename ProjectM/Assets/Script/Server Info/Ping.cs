using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;



public class Ping : MonoBehaviour
{
    public Text Text;
    public Text RText;

    int _cache = -1;
    string Regioninfo;

    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.GetPing() != _cache)
            {
                _cache = PhotonNetwork.GetPing();
                Text.text = _cache.ToString() + " ms";
            }
        }
        else
        {
            if (_cache != -1)
            {
                _cache = -1;
                Text.text = "n/a";
            }
        }
        if (PhotonNetwork.CloudRegion != Regioninfo)
        {
            Regioninfo = PhotonNetwork.CloudRegion;
            if (string.IsNullOrEmpty(Regioninfo))
            {
                RText.text = "n/a";
            }
            else
            {
                RText.text = Regioninfo;
            }
        }
    }


}
