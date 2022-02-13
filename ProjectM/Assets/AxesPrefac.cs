using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AxesPrefac : MonoBehaviour
{
    public Stats Stats;

    Vector3 PosInit;
    [HideInInspector]
    public Vector3 PosLanzamiento;
    float Velocidad;
    public GameObject Egg;
    public GameObject[] puntoHacha;
    public bool active;
    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;

    PhotonView mypv, eggpv;
    void Start()
    {
        PosInit = transform.position;
        mypv = GetComponent<PhotonView>();
        eggpv = Egg.GetComponent<PhotonView>();
    }
    public void Update()
    {



    }
    public void Active()
    {

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            Destroy(Egg);
        }
        else
        {
            StartCoroutine("Instancia");
            //PhotonNetwork.Destroy(Egg);
        }
        active = true;
    }
    IEnumerator Instancia()
    {

        spawn1 = PhotonNetwork.Instantiate("Axe", puntoHacha[0].transform.position, Quaternion.identity);
        spawn2 = PhotonNetwork.Instantiate("Axe", puntoHacha[1].transform.position, Quaternion.identity);
        spawn3 = PhotonNetwork.Instantiate("Axe", puntoHacha[2].transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(2);
        PhotonNetwork.Destroy(mypv);

    }
    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
