using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tornado : MonoBehaviour
{
    [HideInInspector]
    public StatsP StatsP = new StatsP();
    float tiempoattack = 0, countTime = 0, tiempodevida = 0.5f;
    PhotonView myview;
    void Start()
    {
        myview = GetComponent<PhotonView>();
    }
    void Update()
    {
        countTime += Time.deltaTime;
        tiempoattack += Time.deltaTime;
        gameObject.transform.Translate(Vector3.forward * StatsP.velocidad * Time.deltaTime, Space.Self);

        if (tiempoattack > 1)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (countTime >= tiempodevida)
                {
                    Destroy(gameObject);
                }

                Collider[] Enemigos = Physics.OverlapSphere(transform.position, StatsP.HitBoxRadious);

                foreach (var enemigo_ in Enemigos)
                {
                    if (enemigo_ != null)
                    {
                        if (enemigo_.gameObject.layer == 9)
                        {
                            Tower EnemigoT = enemigo_.gameObject.GetComponent<Tower>();
                            if (EnemigoT != null && EnemigoT.Stats.team != StatsP.team)
                            {
                                if (StatsP.team != "")
                                {
                                    Debug.Log("daño a " + enemigo_.gameObject.name);
                                    EnemigoT.Stats.vidacurrent -= StatsP.daño;
                                    Destroy(gameObject);
                                }
                            }
                        }
                        else if (enemigo_.gameObject.layer == 10)
                        {
                            Player Enemigo = enemigo_.gameObject.GetComponent<Player>();
                            if (Enemigo != null && Enemigo.Stats.team != StatsP.team)
                            {
                                if (StatsP.team != "")
                                {
                                    Debug.Log("daño a " + enemigo_.gameObject.name);
                                    Enemigo.Stats.vidacurrent -= StatsP.daño;
                                }
                            }
                        }
                    }
                }
                tiempoattack = 0;
            }
            else
            {
                if (countTime >= tiempodevida && myview.IsMine)
                {
                    PhotonNetwork.Destroy(myview);
                }

                Collider[] Enemigos = Physics.OverlapSphere(transform.position, StatsP.HitBoxRadious);

                foreach (var enemigo_ in Enemigos)
                {
                    if (enemigo_ != null)
                    {
                        if (enemigo_.gameObject.layer == 9)
                        {
                            enemigo_.gameObject.GetComponent<PhotonView>().RPC("RecibirDanoRPC", RpcTarget.All, StatsP.daño);
                            PhotonNetwork.Destroy(gameObject);
                        }
                        else if (enemigo_.gameObject.layer == 10)
                        {
                            enemigo_.gameObject.GetComponent<PhotonView>().RPC("RecibirDanoRPC", RpcTarget.All, StatsP.daño);
                        }
                    }
                }
                tiempoattack = 0;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (Application.isEditor)
        {
            Gizmos.color = StatsP.Objectivo ? Color.green : Color.yellow;
            Gizmos.DrawSphere(transform.position, StatsP.HitBoxRadious);
        }
    }
}
