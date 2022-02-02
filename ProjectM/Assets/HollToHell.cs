using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HollToHell : Player
{
    public PhotonView miView;
    public Player[] personaje;
    public GameObject puntodeRegreso;
    private void Start()
    {
        miView = GetComponent<PhotonView>();
        if (Stats.team=="Red")
        {
            puntodeRegreso = GameObject.Find("puntoRegresoRed");
        }
        else
        {
            puntodeRegreso = GameObject.Find("puntoRegresoblue");
        }
    }
    public void DestroyMiView()
    {
        PhotonNetwork.Destroy(miView);
    }
    override public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==10)
        {
            personaje = other.gameObject.GetComponents<Player>();
            if (personaje[0].Stats.team!=Stats.team)
            {
                personaje[0].transform.position = puntodeRegreso.transform.position;
                DestroyMiView();
            }
        }
    }
}
