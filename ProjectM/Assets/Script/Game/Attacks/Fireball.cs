using UnityEngine;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Fireball : MonoBehaviour
{
    
    public StatsP StatsP = new StatsP();
    PhotonView myview;
    public bool vfx;
    public bool zeus;
    public string vfxList;
    public Transform instanciaVFX;
    public Transform instanciaVFX2;
    public Transform instanciaVFX3;
    public float time;
    void Start()
    {
        myview = GetComponent<PhotonView>();
    }
    void Update()
    {
        time +=Time.deltaTime;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (StatsP.Objectivo)
            {
                if (Vector3.Distance(gameObject.transform.position, StatsP.Objectivo.transform.position) <= StatsP.HitBoxRadious)
                {
                    try
                    {

                        StatsP.Objectivo.GetComponent<Player>().Stats.vidacurrent -= StatsP.daño;
                        Destroy(gameObject);
                        return;
                    }
                    catch (NullReferenceException)
                    {
                    }
                    try
                    {
                        StatsP.Objectivo.GetComponent<Tower>().Stats.vidacurrent -= StatsP.daño;
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
                            StatsP.Objectivo.GetComponent<PhotonView>().RPC("RecibirDanoRPC", RpcTarget.All, StatsP.daño);
                            if (vfx==true)
                            {
                                if (zeus==true)
                                {
                                    PhotonNetwork.Instantiate(vfxList, instanciaVFX.position, instanciaVFX.transform.rotation);
                                    PhotonNetwork.Instantiate(vfxList, instanciaVFX2.position, instanciaVFX2.transform.rotation);
                                    PhotonNetwork.Instantiate(vfxList, instanciaVFX3.position, instanciaVFX3.transform.rotation);
                                    PhotonNetwork.Destroy(myview);
                                }
                                else
                                {
                                    PhotonNetwork.Instantiate(vfxList, instanciaVFX.position, instanciaVFX.transform.rotation);
                                    PhotonNetwork.Destroy(myview);
                                }

                            }
                            else
                            {
                                PhotonNetwork.Destroy(myview);
                            }
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
            if (time>2)
            {
                PhotonNetwork.Destroy(myview);
            }
            else if (!StatsP.Objectivo)
            {
                if (myview.IsMine)
                {
                    PhotonNetwork.Destroy(myview);
                }
                else
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
    IEnumerator VfxActivador()
    {
        PhotonNetwork.Instantiate(vfxList, transform.transform.position, transform.rotation);
        yield return new WaitForSecondsRealtime(1);
        PhotonNetwork.Destroy(myview);
    }
    void OnDrawGizmosSelected()
    {
        if (Application.isEditor)
        {
            Gizmos.color = StatsP.Objectivo ? Color.green : Color.yellow;
            Gizmos.DrawCube(transform.position, transform.localScale * StatsP.HitBoxRadious);
        }
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
