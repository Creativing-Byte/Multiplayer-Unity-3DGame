using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Catapulta : Player
{
    public override void Attack()
    {
        GameObject disparo;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            disparo = Instantiate(balaOffline, transform.position, Quaternion.identity);
        }
        else
        {
            disparo = PhotonNetwork.Instantiate("Fireball", transform.position, Quaternion.identity);
        }
        disparo.GetComponent<Fireball>().StatsP.HitBoxRadious = 2;
        disparo.GetComponent<Fireball>().StatsP.Objectivo = Stats.Objetivo;
        disparo.GetComponent<Fireball>().StatsP.daño = Stats.ataque;
        disparo.GetComponent<Fireball>().StatsP.velocidad = 50f;
    }
    public override void attackEnemy()
    {
        AtaqueTimer += Time.deltaTime;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Stats.Objetivo != null && MyBrain)
            {
                if (Vector3.Distance(Stats.Objetivo.transform.position, gameObject.transform.position) <= (Stats.Range))
                {
                    if (AtaqueTimer >= Stats.vataque)
                    {
                        if (MyAnim)
                        {
                            MyAnim.SetBool("isWalk", false);
                            MyAnim.SetBool("isAttack", true);
                        }

                        if (MyBrain)
                        {
                            if (!MyBrain.isStopped)
                            {
                                MyBrain.isStopped = true;
                            }
                        }

                        Vector3 pos_ = Stats.Objetivo.transform.position;
                        pos_.y = transform.position.y;
                        transform.LookAt(pos_);

                        Attack();

                        AtaqueTimer = 0;
                    }
                }
                else
                {
                    if (MyBrain)
                    {
                        if (MyBrain.isStopped)
                        {
                            MyBrain.isStopped = false;
                        }
                    }

                    Vector3 posTarget_ = Stats.Objetivo.transform.position;
                    MyBrain.SetDestination(posTarget_);
                    MyBrain.speed = Stats.velocidad * 2;
                    MyBrain.autoRepath = true;
                    MyBrain.stoppingDistance = Stats.Range - 1f;

                    if (MyAnim)
                    {
                        MyAnim.SetBool("isAttack", false);
                        MyAnim.SetBool("isWalk", true);
                    }
                }
            }
        }
        else
        {
            if (MyView.IsMine)
            {
                if (Stats.Objetivo != null && MyBrain)
                {

                    if (Vector3.Distance(Stats.Objetivo.transform.position, gameObject.transform.position) <= (Stats.Range))
                    {
                        if (MyView.IsMine && AtaqueTimer >= Stats.vataque)
                        {
                            if (MyAnim)
                            {
                                MyAnim.SetBool("isWalk", false);
                                MyAnim.SetBool("isAttack", true);
                            }

                            if (MyBrain)
                            {
                                if (!MyBrain.isStopped)
                                {
                                    MyBrain.isStopped = true;
                                }
                            }

                            Vector3 pos_ = Stats.Objetivo.transform.position;
                            pos_.y = transform.position.y;
                            transform.LookAt(pos_);

                            Attack();

                            AtaqueTimer = 0;
                        }
                    }
                    else
                    {
                        if (MyBrain)
                        {
                            if (MyBrain.isStopped)
                            {
                                MyBrain.isStopped = false;
                            }
                        }

                        Vector3 posTarget_ = Stats.Objetivo.transform.position;
                        MyBrain.SetDestination(posTarget_);
                        MyBrain.speed = Stats.velocidad * 2;
                        MyBrain.autoRepath = true;
                        MyBrain.stoppingDistance = Stats.Range - 1f;

                        if (MyAnim)
                        {
                            MyAnim.SetBool("isAttack", false);
                            MyAnim.SetBool("isWalk", true);
                        }
                    }
                }
            }
        }
    }
    public override void CheckStatus()
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
                    if (EnemigoT != null && EnemigoT.Stats.team != Stats.team && Stats.Objetivo == null)
                    {
                        if (Stats.Objetivo == null)
                        {
                            Stats.Objetivo = enemigo_.gameObject;
                        }
                        EstadoActual = Status.Ataque;
                        StopMove = Stats.Range;
                        HaveenemyClose = true;
                        return;
                    }
                }
                else if (attackminions && enemigo_.gameObject.layer == 10)
                {
                    Player Enemigo = enemigo_.gameObject.GetComponent<Player>();
                    if (Enemigo != null && Enemigo.Stats.team != Stats.team && Stats.Objetivo == null)
                    {
                        Stats.Objetivo = enemigo_.gameObject;
                        EstadoActual = Status.Ataque;
                        StopMove = Stats.Range;
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
