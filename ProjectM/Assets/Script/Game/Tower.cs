using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour, IPunObservable
{
    public Stats Stats = new Stats();
    public GameObject destruccion;
    public AudioClip DestruccionFx;

    public GameObject BarraVida, balaOffline;

    public float AnchuraMax, AlturaMax;
    float ataquetime = 0;
    PhotonView myview;

    GameObject disparo_, Launcher;

    [HideInInspector]
    public GameObject[] Portal;

    public string PortalTag;
    public string EscombrosTag;

    Collider[] Enemigos;
    public bool IsCentral;

    public AudioClip AttackfxClip;

    public bool AttackBool;

    [Space(10)]
    [SerializeField] private Healthbar health;
    public Camera_Shake BoolActivate1;
    public Camera_Shake BoolActivate2;
    public GameObject Escombros;

    private bool isDestroyed = false;

    void Start()
    {
        myview = GetComponent<PhotonView>();

        Stats.vidacurrent = Stats.vidamax;

        ataquetime = Stats.vataque;

        FindTarget();

        disparo_ = Resources.Load("Disparo") as GameObject;

        Launcher = GameObject.FindGameObjectWithTag("Launcher");

        health.transform.GetChild(0).gameObject.SetActive(true);
        health.SetColor(myview.IsMine);
        if (myview.IsMine)
        {
            if (Stats.team.ToLower() == PhotonInit.MyTeam.ToLower())
            {
                if (PhotonInit.MyTeam.ToLower().Equals("red"))
                    health.transform.localPosition = new Vector3(health.transform.localPosition.x, 4f, health.transform.localPosition.z);
                else health.transform.localPosition = new Vector3(health.transform.localPosition.x, -4f, health.transform.localPosition.z);
            }
        }
    }
    void Update()
    {
        if (isDestroyed) return;

        if (Stats.vidacurrent < Stats.vidamax)
        {
            //health.transform.GetChild(0).gameObject.SetActive(true);
            health.Fill = Stats.vidacurrent / (float)Stats.vidamax;

            if (health.Fill < 0)
            {
                health.Fill = 0;
            }
            if (IsCentral && !AttackBool)
            {
                AttackBool = true;
            }
        }
        //else
        //{
        //    health.transform.GetChild(0).gameObject.SetActive(false);
        //}

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Stats.vidacurrent <= 0)
            {
                if (IsCentral)
                {
                    Time.timeScale = 0;
                    BattleManager.instance.SetWinTutorial(Stats.team);
                    Destroy(gameObject);
                }
                else
                {
                    ActivatedTrigger(gameObject.name);
                    Destroy(gameObject);
                }
            }

            if (ataquetime >= Stats.vataque)
            {
                if (Stats.Objetivo && Vector3.Distance(Stats.Objetivo.transform.position, gameObject.transform.position) <= (Stats.Range))
                {
                    if (IsCentral)
                    {
                        if (AttackBool)
                        {
                            Attack();
                        }
                    }
                    else
                    {
                        Attack();
                    }

                    ataquetime = 0;
                }
                else
                {
                    FindTarget();
                }
            }
            else
            {
                ataquetime += Time.deltaTime;
            }
        }
        else
        {
            if (Stats.vidacurrent <= 0)
            {
                if (Stats.team.ToLower() == "red")
                    BattleManager.instance.bluePoints++;

                if (Stats.team.ToLower() == "blue")
                    BattleManager.instance.redPoints++;

                ActivatedTrigger(gameObject.name);

                if (IsCentral)
                {
                    myview.RPC("WinBattle", RpcTarget.All, Stats.team);
                }
                CameraDestroyShake();
                DestruccionSfx();
                isDestroyed = true;
                destruccion.SetActive(true);
                PhotonNetwork.Instantiate("Explosion", this.transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }

            if (myview.IsMine)
            {
                if (ataquetime >= Stats.vataque)
                {
                    if (Stats.Objetivo && Vector3.Distance(Stats.Objetivo.transform.position, gameObject.transform.position) <= (Stats.Range))
                    {
                        if (IsCentral)
                        {
                            if (AttackBool)
                            {
                                Attack();
                            }
                        }
                        else
                        {
                            Attack();
                        }
                        ataquetime = 0;
                    }
                    else
                    {
                        FindTarget();
                    }
                }
                else
                {
                    ataquetime += Time.deltaTime;
                }
            }
        }
    }
    void FindTarget()
    {
        Enemigos = Physics.OverlapSphere(transform.position, Stats.Range);

        for (int i = 0; i < Enemigos.Length; i++)
        {
            if (Enemigos[i].gameObject.layer == 10)
            {
                try
                {
                    Player Enemigo = Enemigos[i].gameObject.GetComponent<Player>();
                    if (Enemigo.Stats.team != Stats.team)
                    {
                        Stats.Objetivo = Enemigos[i].gameObject;
                        return;
                    }
                }
                catch (NullReferenceException)
                {

                }
            }
        }
    }
    void Attack()
    {
        GameObject disparo;
        AttackSfx();
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            disparo = Instantiate(balaOffline, transform.position, Quaternion.identity);
        }
        else
        {
            disparo = PhotonNetwork.Instantiate("Disparo", transform.position, Quaternion.identity);
        }
        disparo.GetComponent<Projectil>().StatsP.Objectivo = Stats.Objetivo;
        disparo.GetComponent<Projectil>().StatsP.daño = Stats.ataque;
        disparo.GetComponent<Projectil>().StatsP.velocidad = 50f;
    }
    void OnDrawGizmosSelected()
    {
        if (Application.isEditor)
        {
            Gizmos.color = Stats.Objetivo ? Color.green : Color.yellow;
            Gizmos.DrawSphere(transform.position, Stats.Range);
        }
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DañoArea"&&other.GetComponent<Fireball>().StatsP.team!=Stats.team)
        {
            Stats.vidacurrent -= 5;
        }
    }
    void AttackSfx()
    {
        GetComponent<AudioSource>().clip = AttackfxClip;
        GetComponent<AudioSource>().Play();
    }
    void DestruccionSfx()
    {
        GetComponent<AudioSource>().clip = DestruccionFx;
        GetComponent<AudioSource>().Play();
    }

    void CameraDestroyShake()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Camera2");
        GameObject g2 = GameObject.FindGameObjectWithTag("Camera1");
        BoolActivate2 = g.GetComponent<Camera_Shake>();
        BoolActivate2.start = true;
        BoolActivate1 = g2.GetComponent<Camera_Shake>();
        BoolActivate1.start = true;
    }
    void ActivateEscombros()
    {
        GameObject escombro = GameObject.FindGameObjectWithTag(EscombrosTag);
        Escombros = escombro;
        Escombros.SetActive(true);
    }
    void ActivatedTrigger(string gmname)
    {
        string Team = Stats.team == "Red" ? "Blue" : "Red";
        GameObject Trigger = GameObject.FindGameObjectWithTag("Triggers" + Team);
        if (gmname == "Tower" + Stats.team + "Izq(Clone)")
        {
            Trigger.transform.GetChild(2).GetComponent<Trigger>().Direccion = null;
            Trigger.transform.GetChild(2).GetComponent<Trigger>().Active = true;
        }
        else if (gmname == "Tower" + Stats.team + "Der(Clone)")
        {
            Trigger.transform.GetChild(4).GetComponent<Trigger>().Direccion = null;
            Trigger.transform.GetChild(4).GetComponent<Trigger>().Active = true;
        }
    }
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
    [PunRPC]
    public void RecibirDanoRPC(int f_)
    {
        Stats.vidacurrent -= f_;
    }
    [PunRPC]
    public void WinBattle(string TeamLoser)
    {
        Time.timeScale = 0;
        if (myview.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        BattleManager.instance.EndGame();
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSecondsRealtime(2);
        PhotonNetwork.Destroy(gameObject);
    }
}
