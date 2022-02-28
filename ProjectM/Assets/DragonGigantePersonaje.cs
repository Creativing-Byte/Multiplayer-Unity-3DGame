using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class DragonGigantePersonaje : Player
{
    public DragonGigante VelociraptorClone;
    public PhotonView velociraptoview;
    public PhotonView miView;
    public MoveLanzables moveLanzable;
    public GameObject puntoVfx;
    public bool activacion;
    void Awake()
    {
        moveLanzable = GetComponent<MoveLanzables>();
        velociraptoview = GameObject.Find("DragonGigante(Clone)").GetComponent<PhotonView>();
        miView = GetComponent<PhotonView>();
        if (velociraptoview.IsMine)
        {
            VelociraptorClone = velociraptoview.gameObject.GetComponent<DragonGigante>();
            Stats.team = VelociraptorClone.Stats.team;
            Stats.vidamax = VelociraptorClone.Stats.vidamax;
            Stats.vidacurrent = VelociraptorClone.Stats.vidacurrent;
            Stats.ataque = VelociraptorClone.Stats.ataque;
            Stats.velocidad = VelociraptorClone.Stats.velocidad;
            Stats.vataque = VelociraptorClone.Stats.vataque;
            Stats.Range = VelociraptorClone.Stats.Range;
            Stats.Objetivo = VelociraptorClone.Stats.Objetivo;
            Stats.TipoGround = VelociraptorClone.Stats.TipoGround;
            Stats.TipoFlying = VelociraptorClone.Stats.TipoFlying;
            Stats.Ground = VelociraptorClone.Stats.Ground;
            Stats.Flying = VelociraptorClone.Stats.Flying;
        }
        GetComponent<NavMeshAgent>().stoppingDistance = Stats.Range - 1f;
    }
    public void Vfx()
    {
        PhotonNetwork.Instantiate("LegendaryDragonFire", punchVFX.transform.position, Quaternion.identity);
        moveLanzable.velocidad = 0;
        StartCoroutine("Attaque");

    }
    IEnumerator Attaque()
    {
        yield return new WaitForSecondsRealtime(1);
        PhotonNetwork.Destroy(miView);
    }
}
