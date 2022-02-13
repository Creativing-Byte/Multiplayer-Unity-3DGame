using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Axes : Player
{

    public AxesPrefac VelociraptorClone;
    public PhotonView velociraptoview;
    public PhotonView miView;
    public MoveLanzables moveLanzable;
    public bool activacion;
    public MeshRenderer mesh;
    void Awake()
    {
        moveLanzable = GetComponent<MoveLanzables>();
        mesh = GetComponent<MeshRenderer>();
        velociraptoview = GameObject.Find("HachasPrefac(Clone)").GetComponent<PhotonView>();
        miView = GetComponent<PhotonView>();
        if (velociraptoview.IsMine)
        {
            VelociraptorClone = velociraptoview.gameObject.GetComponent<AxesPrefac>();
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
        PhotonNetwork.Instantiate("AxesPunch", transform.transform.position, Quaternion.identity);
        moveLanzable.velocidad = 0;
        mesh.enabled = false;
        StartCoroutine("Attaque");

    }
    IEnumerator Attaque()
    {
        yield return new WaitForSecondsRealtime(1);
        PhotonNetwork.Destroy(miView);
    }
}
