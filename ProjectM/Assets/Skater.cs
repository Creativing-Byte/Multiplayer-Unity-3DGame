using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Skater : Player
{
    [HideInInspector]
    public int enemiesInRange = 0;
    public GameObject PosDisparo;
    public GameObject Fireball;
    //public GameObject arma;
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

    override public void Attack()
    {
        GameObject disparo;
        SoundSfx();
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            //arma.SetActive(true);
            disparo = Instantiate(Fireball, PosDisparo.transform.position, Quaternion.identity);
            disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
            disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
            disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
            disparo.GetComponent<Fireball>().StatsP.velocidad = 100f;
            //StartCoroutine("ActiveArma");
        }
        else
        {
            //arma.SetActive(true);
            disparo = PhotonNetwork.Instantiate("Fireball", PosDisparo.transform.position, Quaternion.identity);
            disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
            disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
            disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
            disparo.GetComponent<Fireball>().StatsP.velocidad = 100f;
            //StartCoroutine("ActiveArma");
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
    /*IEnumerator ActiveArma()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        arma.SetActive(false);

    }*/
}
