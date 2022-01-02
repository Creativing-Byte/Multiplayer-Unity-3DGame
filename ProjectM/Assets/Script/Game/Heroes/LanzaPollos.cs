using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanzaPollos : Player
{
    public GameObject PolloPrefabs, PosDisparo;
    int PollosStacks = 6;
    void Update()
    {
        CheckStatus();

        if (!HaveenemyClose)
        {
            Walk();
        }
        else
        {
            attackEnemy();
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Stats.vidacurrent <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (Stats.vidacurrent <= 0)
            {
                if (MyView.IsMine)
                    PhotonNetwork.Destroy(MyView);
            }
        }
    }

    override public void Attack()
    {
        if (PollosStacks < 1)
        {
            GameObject Disparo;
            SoundSfx();
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                Disparo = Instantiate(balaOffline, PosDisparo.transform.position, Quaternion.identity);
                Disparo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                Disparo = PhotonNetwork.Instantiate("Disparo", PosDisparo.transform.position, Quaternion.identity);
                Disparo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            Disparo.GetComponent<Projectil>().StatsP.Objectivo = Stats.Objetivo;
            Disparo.GetComponent<Projectil>().StatsP.daño = Stats.ataque;
            Disparo.GetComponent<Projectil>().StatsP.velocidad = 150f;
        }
        else
        {
            GameObject Pollo;
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                Pollo = Instantiate(PolloPrefabs, PosDisparo.transform.position, Quaternion.identity);
            }
            else
            {
                Pollo = PhotonNetwork.Instantiate("Pollo", PosDisparo.transform.position, Quaternion.identity);
            }
            Pollo.GetComponent<Pollo>().Stats.team = Stats.team;
            Pollo.GetComponent<Pollo>().Stats.Objetivo = Stats.Objetivo;
            Pollo.GetComponent<Pollo>().Stats.Range = 25;
            PollosStacks -= 1;
            if (PollosStacks < 1)
            {
                Stats.Objetivo = null;
            }
        }
        
    }
    override public void Walk()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Stats.velocidad == 0 || EstadoActual == Status.Inactivo)
            {
                if (MyAnim)
                {
                    MyAnim.SetBool("isWalk", false);
                    MyAnim.SetBool("isAttack", false);
                    MyAnim.SetBool("isThrow", false);
                }
            }
            else
            {
                if (Destino != null && MyBrain)
                {
                    if (MyBrain.isStopped)
                    {
                        MyBrain.isStopped = false;
                    }

                    MyBrain.SetDestination(DestinoTarget);
                    MyBrain.speed = Stats.velocidad * 2;
                    MyBrain.autoRepath = true;
                    MyBrain.stoppingDistance = Stats.Range - 1f;

                    if (MyAnim)
                    {
                        MyAnim.SetBool("isWalk", true);
                        MyAnim.SetBool("isAttack", false);
                        MyAnim.SetBool("isThrow", false);
                    }
                }
            }
        }
        else
        {
            if (MyView.IsMine)
            {
                if (Stats.velocidad == 0 || EstadoActual == Status.Inactivo)
                {
                    if (MyAnim)
                    {
                        MyAnim.SetBool("isWalk", false);
                        MyAnim.SetBool("isAttack", false);
                        MyAnim.SetBool("isThrow", false);
                    }
                }
                else
                {
                    if (Destino != null && MyBrain)
                    {
                        if (MyBrain.isStopped)
                        {
                            MyBrain.isStopped = false;
                        }

                        MyBrain.SetDestination(DestinoTarget);
                        MyBrain.speed = Stats.velocidad * 2;
                        MyBrain.autoRepath = true;
                        MyBrain.stoppingDistance = Stats.Range - 1f;

                        if (MyAnim)
                        {
                            MyAnim.SetBool("isWalk", true);
                            MyAnim.SetBool("isAttack", false);
                            MyAnim.SetBool("isThrow", false);
                        }
                    }
                }
            }
        }
    }
    override public void attackEnemy()
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
                            if (PollosStacks < 1)
                            {
                                MyAnim.SetBool("isAttack", true);
                                MyAnim.SetBool("isThrow", false);  
                            }
                            else
                            {
                                MyAnim.SetBool("isThrow", true);
                                MyAnim.SetBool("isAttack", false);
                            }
                            MyAnim.SetBool("isWalk", false);
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
                        MyAnim.SetBool("isWalk", true);
                        MyAnim.SetBool("isAttack", false);
                        MyAnim.SetBool("isThrow", false);
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
                                if (PollosStacks < 1)
                                {
                                    MyAnim.SetBool("isAttack", true);
                                    MyAnim.SetBool("isThrow", false);
                                }
                                else
                                {
                                    MyAnim.SetBool("isThrow", true);
                                    MyAnim.SetBool("isAttack", false);
                                }
                                MyAnim.SetBool("isWalk", false);
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
                            MyAnim.SetBool("isWalk", true);
                            MyAnim.SetBool("isAttack", false);
                            MyAnim.SetBool("isThrow", false);
                        }
                    }
                }
            }
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
                        Stats.Objetivo = enemigo_.gameObject;
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
