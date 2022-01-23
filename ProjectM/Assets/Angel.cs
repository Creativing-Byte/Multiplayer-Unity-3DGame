using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class Angel : Player
{
    [HideInInspector]
    public int enemiesInRange = 0;
    public GameObject PosDisparo;
    public GameObject Fireball, Tornado;
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
    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("AngelShoot", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        StartCoroutine("DestroyMIVfx");
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

            disparo = PhotonNetwork.Instantiate("Fireball", PosDisparo.transform.position, Quaternion.identity);
            disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
            disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
            disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
            disparo.GetComponent<Fireball>().StatsP.velocidad = 100f;


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
