using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Pollo : Player
{
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
        //if (Stats.vidacurrent < Stats.vidamax)
        //{
        //    BarraVida.SetActive(true);
        //    BarraVida.transform.localScale = new Vector3(((float)Stats.vidacurrent / (float)Stats.vidamax * AnchuraMax), BarraVida.transform.localScale.y, BarraVida.transform.localScale.z);
        //    if (BarraVida.transform.localScale.x < 0)
        //    {
        //        BarraVida.transform.localScale = Vector3.zero;
        //    }
        //}
        //else
        //{
        //    BarraVida.SetActive(false);
        //}
        //BarraVida.transform.position = gameObject.transform.position + Vector3.up * AlturaMax;
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
}
