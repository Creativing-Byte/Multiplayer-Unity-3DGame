using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

public class Ganster : Player
{
    [HideInInspector]
    public int enemiesInRange = 0;
    public GameObject PosDisparo;
    public GameObject Fireball, Tornado, Explosion,personaje,vfxExplosion;
    void Start()
    {
        MyBrain = GetComponent<NavMeshAgent>();
        MyView = GetComponent<PhotonView>();
        MyAnim = GetComponent<Animator>();
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        Stats.vidacurrent = Stats.vidamax;
        AtaqueTimer = Stats.vataque;
        GetComponentInChildren<SphereCollider>().radius = Stats.Range;
    }
    void LateUpdate()
    {
        if (Stats.vidacurrent <= 51)
        {
            personaje.SetActive(false);
            GameObject
            disparo = PhotonNetwork.Instantiate("Explosion", PosDisparo.transform.position, Quaternion.identity);
            disparo.GetComponent<Explosion>().StatsP.HitBoxRadious = 10;
            disparo.GetComponent<Explosion>().StatsP.Objectivo = Stats.Objetivo;
            disparo.GetComponent<Explosion>().StatsP.daño = Stats.ataque;
            disparo.GetComponent<Explosion>().StatsP.velocidad = 0f;
            Stats.vidacurrent = 0;
            
        }
    }

    override public void CheckStatus()
    {
        timerCheck += Time.deltaTime;
        if (timerCheck < 0.25f)
            return;

        timerCheck = 0;

        Collider[] Enemigos = Physics.OverlapSphere(transform.position, Stats.Range);

        foreach (var enemigo_ in Enemigos)
        {
            if (enemigo_ != null)
            {
                if (enemigo_.gameObject.layer == 9)
                {
                    Tower EnemigoT = enemigo_.gameObject.GetComponent<Tower>();
                    if (EnemigoT != null && EnemigoT.Stats.team != Stats.team)
                    {
                        if (Stats.Objetivo == null)
                        {
                            Stats.Objetivo = enemigo_.gameObject;
                        }
                        EstadoActual = Status.Ataque;
                        HaveenemyClose = true;
                        return;
                    }
                }
                else if (attackminions && enemigo_.gameObject.layer == 10)
                {
                    Player Enemigo = enemigo_.gameObject.GetComponent<Player>();
                    if (Enemigo != null && Enemigo.Stats.team != Stats.team)
                    {
                        if (Stats.Objetivo == null)
                        {
                            Stats.Objetivo = enemigo_.gameObject;
                        }
                        EstadoActual = Status.Ataque;
                        HaveenemyClose = true;
                        return;
                    }
                }
            }
        }
        if (Stats.Objetivo == null)
        {
            HaveenemyClose = false;
        }
    }
}
