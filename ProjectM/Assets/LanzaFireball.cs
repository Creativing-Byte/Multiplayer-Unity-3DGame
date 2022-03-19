using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LanzaFireball : Player
{
    public FireballInsta VelociraptorClone;
    public PhotonView velociraptoview;
    public PhotonView miView;
    public bool activacion;
    [HideInInspector]
    public int enemiesInRange = 0;
    public GameObject PosDisparo;
    public GameObject Fireball, Tornado;
    public GameObject fireballVFX;
    public GameObject target;
    void Awake()
    {
      
        velociraptoview = GameObject.Find("FireballLanzador(Clone)").GetComponent<PhotonView>();
        miView = GetComponent<PhotonView>();
        if (velociraptoview.IsMine)
        {
            VelociraptorClone = velociraptoview.gameObject.GetComponent<FireballInsta>();
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

    public override void Walk()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1);
    }
    public override void attackEnemy()
    {
        MyAnim.SetBool("isWalk", false);
        MyAnim.SetBool("isAttack", true);
    }

    public override void Punch()
    {
        PhotonNetwork.Instantiate("FireBallVfx", transform.transform.position, Quaternion.identity);
        //StartCoroutine("DestroyMIVfx");
    }
    public void DestLanzador()
    {
        target.GetComponentInParent<DestroyPunch>().DestroyMiView();
    }

    IEnumerator Attaque()
    {
        yield return new WaitForSecondsRealtime(1);
        PhotonNetwork.Destroy(miView);
    }
    override public void Attack()
    {
        GameObject disparo;
        SoundSfx();
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (enemiesInRange > 1)
            {
                disparo = Instantiate(Tornado, PosDisparo.transform.position, transform.rotation);
                disparo.GetComponent<Tornado>().StatsP.team = Stats.team;
                disparo.GetComponent<Tornado>().StatsP.HitBoxRadious = 25;
                disparo.GetComponent<Tornado>().StatsP.Objectivo = Stats.Objetivo;
                disparo.GetComponent<Tornado>().StatsP.daño = Stats.ataque;
                disparo.GetComponent<Tornado>().StatsP.velocidad = 100f;
            }
            else
            {
                disparo = Instantiate(Fireball, PosDisparo.transform.position, Quaternion.identity);
                disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
                disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
                disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
                disparo.GetComponent<Fireball>().StatsP.velocidad = 100f;
            }

        }
        else
        {
            fireballVFX.SetActive(false);
            disparo = PhotonNetwork.Instantiate("Fireball", transform.transform.position, Quaternion.identity);
            disparo.GetComponent<Fireball>().StatsP.team = Stats.team;
            disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
            disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
            disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
            disparo.GetComponent<Fireball>().StatsP.velocidad = 100f;
        }
    }
}
