using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPunObservable
{
    public Stats Stats = new Stats();
    public GameObject Destino;
    public Transform punchVFXpuntoi;

    protected Animator MyAnim;

    protected Status EstadoActual = Status.Moviendo;

    public bool attackminions;

    public GameObject balaOffline;
    protected GameObject Launcher;

    protected bool HaveenemyClose;

    [HideInInspector]
    public Vector3 DestinoTarget;
    [HideInInspector]
    public float StopMove = 20;

    protected float timerCheck = 0, AtaqueTimer;

    public float AnchuraMax, AlturaMax;

    protected PhotonView MyView;

    protected NavMeshAgent MyBrain;
    protected enum Status { Inactivo, Ataque, Moviendo, Idle }

    public AudioClip Sfx;

    private bool firstLoad = true;
    public GameObject punchVFX;
    [Space(10)]
    [SerializeField] private Healthbar health;

    void Start()
    {
        TryGetComponent(out MyBrain);
        TryGetComponent(out MyView);
        TryGetComponent(out MyAnim);
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        Stats.vidacurrent = Stats.vidamax;
        AtaqueTimer = Stats.vataque;

        health.SetColor(MyView.IsMine);
    }
    //-------------------------------------------
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

        if (Stats.vidacurrent < Stats.vidamax)
        {
            health.transform.GetChild(0).gameObject.SetActive(true);
            //BarraVida.transform.localScale = new Vector3(((float)Stats.vidacurrent / (float)Stats.vidamax * AnchuraMax), BarraVida.transform.localScale.y, BarraVida.transform.localScale.z);
            health.Fill = Stats.vidacurrent / (float)Stats.vidamax;

            if (health.Fill < 0)
            {
                //  BarraVida.transform.localScale = Vector3.zero;
                health.Fill = 0;
            }
        }
        else health.transform.GetChild(0).gameObject.SetActive(false);
    }
    //-------------------------------------------
    public virtual void Walk()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Stats.velocidad == 0 || EstadoActual == Status.Inactivo)
            {
                if (MyAnim)
                {
                    MyAnim.SetBool("isWalk", false);
                    MyAnim.SetBool("isAttack", false);
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
                if (Stats.velocidad == 0 || EstadoActual == Status.Inactivo)
                {
                    if (MyAnim)
                    {
                        MyAnim.SetBool("isWalk", false);
                        MyAnim.SetBool("isAttack", false);
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
                            MyAnim.SetBool("isAttack", false);
                            MyAnim.SetBool("isWalk", true);
                        }
                    }
                }
            }
        }
    }
    public virtual void attackEnemy()
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = HaveenemyClose ? Color.green : Color.yellow;
        Gizmos.DrawSphere(transform.position, Stats.Range);
    }
    public virtual void CheckStatus()
    {
        if (!firstLoad)
        {
            timerCheck += Time.deltaTime;
            if (timerCheck < 0.25f)
                return;
        }

        firstLoad = false;
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
                        Stats.Objetivo = enemigo_.gameObject;
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
    public virtual void SoundSfx()
    {
        GetComponent<AudioSource>().clip = Sfx;
        GetComponent<AudioSource>().Play();
    }
    public virtual void Punch()
    {
        
        punchVFX=PhotonNetwork.Instantiate("Punch", punchVFXpuntoi.transform.position, Quaternion.Euler(90,0,0));
        StartCoroutine("DestroyMIVfx");

    }

    public IEnumerator DestroyMIVfx()
    {
        yield return new WaitForSecondsRealtime(1);
        PhotonNetwork.Destroy(punchVFX);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TimeStop")
        {
            StartCoroutine("TimeStoped");
        }
    }
    [PunRPC]
    IEnumerator TimeStoped()
    {
        Stats.Objetivo = null;
        Stats.velocidad = 0;
        Stats.Range = 0;
        yield return new WaitForSecondsRealtime(2);
        Stats = new Stats();

    }

    //-------------------------------------------
    public virtual void Attack()
    {
        GameObject disparo;
        SoundSfx();
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            disparo = Instantiate(balaOffline, transform.position, Quaternion.identity);
            disparo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        else
        {
            disparo = PhotonNetwork.Instantiate("Disparo", transform.position, Quaternion.identity);
        }
        disparo.GetComponent<Projectil>().StatsP.Objectivo = Stats.Objetivo;
        disparo.GetComponent<Projectil>().StatsP.daño = Stats.ataque;
        disparo.GetComponent<Projectil>().StatsP.velocidad = 150f;
    }
    //-------------------------------------------
    [PunRPC]
    public void RecibirDanoRPC(int f_)
    {
        Stats.vidacurrent -= f_;
    }
    //-------------------------------------------
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Stats.vidamax);
            stream.SendNext(Stats.vidacurrent);
        }
        else if (stream.IsReading)
        {
            Stats.vidamax = (int)stream.ReceiveNext();
            Stats.vidacurrent = (int)stream.ReceiveNext();
        }
    }
}