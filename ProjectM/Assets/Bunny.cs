using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bunny : Player
{
    public GameObject cuerpo;
    public GameObject cabeza;
    public GameObject vfxMovent;
    public GameObject bunnyCol;
    public GameObject bunnyTMS;
    void VfxMovent()
    {
        cuerpo.SetActive(false);
        cabeza.SetActive(false);
        //bunnyCol.GetComponent<BoxCollider>().enabled = false;
        vfxMovent.SetActive(true);
        bunnyTMS.SetActive(true);
    }
    void VfxMoventDesactived()
    {
        cuerpo.SetActive(true);
        cabeza.SetActive(true);
        //bunnyCol.GetComponent<BoxCollider>().enabled = true;
        vfxMovent.SetActive(false);
        bunnyTMS.SetActive(false);
    }
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("BunnyPunch", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
    }
}
