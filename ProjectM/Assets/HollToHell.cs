using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HollToHell : Player
{
    public GameObject personajes;
    public GameObject personaje;
    public int destroy;
    public float time;
    public override void attackEnemy()
    {
        MyAnim.SetBool("isWalk", false);
        MyAnim.SetBool("isAttack", true);
    }
    override public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>().Stats.team != gameObject.GetComponent<Player>().Stats.team)
        {
            personajes = other.gameObject;

            if (personajes.gameObject.GetComponent<Player>().Stats.team == "Red")
            {
                personaje = PhotonNetwork.Instantiate(personajes.gameObject.GetComponent<Player>().prefac, personajes.gameObject.GetComponent<Player>().posInicial.transform.position, Quaternion.Euler(0, 180, 0));

            }
            else
            {
                personaje = PhotonNetwork.Instantiate(personajes.gameObject.GetComponent<Player>().prefac, personajes.gameObject.GetComponent<Player>().posInicial.transform.position, Quaternion.identity);
            }
            personaje.GetComponent<Player>().prefac = personajes.gameObject.GetComponent<Player>().prefac;
            personaje.GetComponent<Player>().Stats.team = personajes.gameObject.GetComponent<Player>().Stats.team;
            personaje.GetComponent<Player>().Stats.vidamax = personajes.gameObject.GetComponent<Player>().Stats.vidamax;
            personaje.GetComponent<Player>().Stats.vidacurrent = personajes.gameObject.GetComponent<Player>().Stats.vidacurrent;
            personaje.GetComponent<Player>().Stats.ataque = personajes.gameObject.GetComponent<Player>().Stats.ataque;
            personaje.GetComponent<Player>().Stats.velocidad = personajes.gameObject.GetComponent<Player>().Stats.velocidad;
            personaje.GetComponent<Player>().Stats.vataque = personajes.gameObject.GetComponent<Player>().Stats.vataque;
            personaje.GetComponent<Player>().Stats.Range = personajes.gameObject.GetComponent<Player>().Stats.Range;
            destroy = 5;
            
            
            
        }
    }
    public void Update()
    {
        time += Time.deltaTime;
        if (personaje!=null)
        {
            PhotonNetwork.Destroy(personajes.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
        if (time>=30)
        {
            PhotonNetwork.Destroy(personajes.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
