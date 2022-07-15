using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HollToHellPrefac : MonoBehaviour
{
    public DestroyPunch destroy;
    private void Start()
    {
        destroy = GetComponent<DestroyPunch>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>().Stats.team !=gameObject.GetComponent<Player>().Stats.team)
        {
            GameObject personaje;
            if (other.gameObject.GetComponent<Player>().Stats.team == "Red")
            {
                personaje = PhotonNetwork.Instantiate(other.gameObject.GetComponent<Player>().prefac, other.gameObject.GetComponent<Player>().posInicial.transform.position, Quaternion.Euler(0, 180, 0));

            }
            else
            {
                personaje = PhotonNetwork.Instantiate(other.gameObject.GetComponent<Player>().prefac, other.gameObject.GetComponent<Player>().posInicial.transform.position, Quaternion.identity);
            }
            personaje.GetComponent<Player>().prefac = other.gameObject.GetComponent<Player>().prefac;
            personaje.GetComponent<Player>().Stats.team = other.gameObject.GetComponent<Player>().Stats.team;
            personaje.GetComponent<Player>().Stats.vidamax = other.gameObject.GetComponent<Player>().Stats.vidamax;
            personaje.GetComponent<Player>().Stats.vidacurrent = other.gameObject.GetComponent<Player>().Stats.vidacurrent;
            personaje.GetComponent<Player>().Stats.ataque = other.gameObject.GetComponent<Player>().Stats.ataque;
            personaje.GetComponent<Player>().Stats.velocidad = other.gameObject.GetComponent<Player>().Stats.velocidad;
            personaje.GetComponent<Player>().Stats.vataque = other.gameObject.GetComponent<Player>().Stats.vataque;
            personaje.GetComponent<Player>().Stats.Range = other.gameObject.GetComponent<Player>().Stats.Range;
            PhotonNetwork.Destroy(other.gameObject);
            destroy.DestroyMiView();
            print("446444464654");
        }
    }
}
