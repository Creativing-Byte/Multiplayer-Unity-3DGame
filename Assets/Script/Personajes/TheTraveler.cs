using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class TheTraveler : Player
{
    public GameObject explosionContador;
    [HideInInspector]
    public int enemiesInRange = 0;
    public GameObject PosDisparo;
    public GameObject Fireball;
    public int destroyper;
    public DestroyPunch destroid;
    public int velociodad;
    //public GameObject arma;
    public void Awake()
    {
        velocidad = Stats.velocidad;
    }

    public override void Punch()
    {
        punchVFX = PhotonNetwork.Instantiate("TravelerZone", punchVFXpuntoi.transform.position, punchVFXpuntoi.rotation);
        destroyper++;
        //StartCoroutine("DestroyMIVfx");
    }
    public void LateUpdate()
    {
        if (destroyper==2)
        {
            destroid.DestroyMiView();
        }
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
            StartCoroutine("ActiveArma");
        }
        else
        {
            //arma.SetActive(true);
            disparo = PhotonNetwork.Instantiate("TimeStopTraveler", PosDisparo.transform.position, Quaternion.identity);
            disparo.GetComponent<Fireball>().StatsP.team = Stats.team;
            disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
            disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
            disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
            disparo.GetComponent<Fireball>().StatsP.velocidad = 0f;
            StartCoroutine("ActiveArma");
        }
    }
    public void ExplosionMTD()
    {
        explosionContador.SetActive(true);
        Stats.velocidad = 0;   
    }
    public void ReturnVelocida()
    {
        Stats.velocidad = velocidad;
        MyAnim.SetBool("isWalk", true);
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
