using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class FireballInsta : MonoBehaviour
{
    public Stats Stats;
    public GameObject PosInit;
    public GameObject target;
    public GameObject PosLanzamiento;
    float Velocidad;
    public GameObject[] Velociraptor;
    public bool active;
    public GameObject spawv;
    public float time;
    PhotonView mypv, eggpv;
    void Start()
    {

        PosInit =this.gameObject;
        mypv = GetComponent<PhotonView>();
        
    }
    public void Update()
    {
        if (PosLanzamiento==null)
        {
            if (Stats.team == "Blue")
            {
                PosLanzamiento = GameObject.Find("TowerBlueMid(Clone)").GetComponent<Tower>().canonInst;
            }
            else if (Stats.team == "Red")
            {
                PosLanzamiento= GameObject.Find("TowerRedMid(Clone)").GetComponent<Tower>().canonInst;
            }
        }
        if (active==true)
        {
            spawv = PhotonNetwork.Instantiate("FireBallPrefac", PosLanzamiento.transform.position, Quaternion.Euler(0, 180, 0));
            spawv.gameObject.GetComponent<LanzaFireball>().target = target;
            active = false;
        }
        time += Time.deltaTime;
        if (time>5)
        {
            PhotonNetwork.Destroy(mypv);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== "FireBallPrefac(Clone)")
        {
            print("ffff");
            if (other.gameObject.GetComponent<LanzaFireball>().Stats.team == Stats.team)
            {
                if (other.gameObject.GetComponent<LanzaFireball>().Stats.Objetivo == null)
                {
                    spawv.gameObject.GetComponent<LanzaFireball>().attackEnemy();
                    PhotonNetwork.Destroy(mypv);
                }

            }
        }
    }
    /*public void Active()
    {
        spawv = PhotonNetwork.Instantiate("FireBallPrefac", PosLanzamiento.transform.position, Quaternion.Euler(0, 180, 0));
        spawv.gameObject.GetComponent<LanzaFireball>().target = PosInit;
    }*/

    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
    
}
