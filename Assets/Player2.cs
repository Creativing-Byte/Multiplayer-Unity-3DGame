using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class Player2 : MonoBehaviour//IPunObservable
{
    public Stats stats = new Stats();
    public GameObject destino;
    public Animator MyAnim;
    [SerializeField]
    protected Status EstadoActual = Status.Moviendo;
    protected enum Status { Inactivo, Ataque, Moviendo, Idle }
    public bool attackminions;
    public GameObject balaOffline;
    protected GameObject Launcher;
    protected bool HaveenemyClose;
    public Vector3 DestinoTarget;
    public float StopMove = 20;
    public float timerCheck = 0, AtaqueTimer;
    public float AnchuraMax, AlturaMax;
    protected PhotonView MyView;
    protected NavMeshAgent MyBrain;
    public AudioClip Sfx;
    private bool firstLoad = true;
    [Space(10)]
    [SerializeField] private Healthbar health;
    public Collider[] Enemigos;
    public List<Tower> listadeTorres = new List<Tower>();
    public Player[] listaEnemigos;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out MyBrain);
        TryGetComponent(out MyView);
        TryGetComponent(out MyAnim);
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        stats.vidacurrent = stats.vidamax;
        AtaqueTimer = stats.vataque;
        health.SetColor(MyView.IsMine);
    }

    void Update()
    {
    }
    public virtual void CheckStatus()
    {

        Enemigos = Physics.OverlapSphere(transform.position, stats.Range);
        for (int i = 0; i < Enemigos.Length; i++)
        {
            if (Enemigos[i].gameObject.layer==9|| Enemigos[i].gameObject.layer == 10)
            {
                //listadeTorres[i]=Enemigos[i].gameObject.GetComponents<Tower>();

            }
        }
        
    }

}
