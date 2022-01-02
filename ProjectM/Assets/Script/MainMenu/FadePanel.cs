using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class FadePanel : MonoBehaviour
{
    public GameObject go;
    public Image[] image = new Image[3];
    public PhotonView pv;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        try
        {
            if (pv.IsMine)
            {
                go.SetActive(true);
            }
        }
        catch (NullReferenceException)
        {
        }
    }
    public void Fade()
    {
        for (int i = 0; i < image.Length; i++)
        {
            var tempColor = image[i].color;
            tempColor.a = 0;
            image[i].color = tempColor;
        }

        if (pv.IsMine)
        {
            Destroy(go);
        }
    }
    [PunRPC]
    public async void FadeOnline()
    {
        GameObject pj1 = null;
        GameObject pj2 = null;
        do
        {
            if(!pj1) pj1 = GameObject.FindGameObjectWithTag("Camera1");
            if(!pj2) pj2 = GameObject.FindGameObjectWithTag("Camera2");
            await Task.Yield();
        }
        while (!pj1 || !pj2);

        if (pj1.GetComponent<PhotonView>().IsMine)
        {
            pj1.GetComponentInChildren<FadePanel>().Fade();
        }
        else if (pj2.GetComponent<PhotonView>().IsMine)
        {
            pj2.GetComponentInChildren<FadePanel>().Fade();
        }
    }
}
