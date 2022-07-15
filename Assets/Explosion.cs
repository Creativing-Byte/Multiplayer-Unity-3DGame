using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    [HideInInspector]
    public StatsP StatsP = new StatsP();
    PhotonView myview;
    void Start()
    {
        myview = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (StatsP.Objectivo)
            {
                if (Vector3.Distance(gameObject.transform.position, StatsP.Objectivo.transform.position) <= StatsP.HitBoxRadious)
                {
                    try
                    {

                        StatsP.Objectivo.GetComponent<Player>().Stats.vidacurrent -= StatsP.da�o;
                        Destroy(gameObject);
                        return;
                    }
                    catch (NullReferenceException)
                    {
                    }
                    try
                    {
                        StatsP.Objectivo.GetComponent<Tower>().Stats.vidacurrent -= StatsP.da�o;
                        Destroy(gameObject);
                        return;
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
                else
                {
                    gameObject.transform.LookAt(StatsP.Objectivo.transform);
                    gameObject.transform.Translate(Vector3.forward * StatsP.velocidad * Time.deltaTime, Space.Self);
                }
            }
            else if (StatsP.Objectivo == null)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (StatsP.Objectivo)
            {
                if (Vector3.Distance(gameObject.transform.position, StatsP.Objectivo.transform.position) <= StatsP.HitBoxRadious)
                {
                    try
                    {
                        if (myview.IsMine)
                        {
                            StatsP.Objectivo.GetComponent<PhotonView>().RPC("RecibirDanoRPC", RpcTarget.All, StatsP.da�o);
                            PhotonNetwork.Destroy(myview);
                        }
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
                else
                {
                    gameObject.transform.LookAt(StatsP.Objectivo.transform);
                    gameObject.transform.Translate(Vector3.forward * StatsP.velocidad * Time.deltaTime);
                }
            }
            else if (!StatsP.Objectivo)
            {
                if (myview.IsMine)
                {
                    PhotonNetwork.Destroy(myview);
                }
                else
                {
                    //PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
    void Destruccion()
    {
        PhotonNetwork.Destroy(myview);
    }
    void OnDrawGizmosSelected()
    {

            Gizmos.color = StatsP.Objectivo ? Color.green : Color.yellow;
            Gizmos.DrawCube(transform.position, transform.localScale * StatsP.HitBoxRadious);

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(StatsP.Objectivo);
        }
        else if (stream.IsReading)
        {
            StatsP.Objectivo = (GameObject)stream.ReceiveNext();
        }
    }
}