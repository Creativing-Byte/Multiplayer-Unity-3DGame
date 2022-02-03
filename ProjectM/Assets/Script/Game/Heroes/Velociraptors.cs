using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Velociraptors : MonoBehaviour
{
    
    public Stats Stats;

    Vector3 PosInit;
    [HideInInspector]
    public Vector3 PosLanzamiento;
    float Velocidad;
    public GameObject Egg;
    public GameObject[] Velociraptor;
    bool active;

    PhotonView mypv, eggpv;
    void Start()
    {
        PosInit = transform.position;
        mypv = GetComponent<PhotonView>();
        eggpv = Egg.GetComponent<PhotonView>();
    }
    public void Update()
    {
        Velocidad += Time.deltaTime;
        Velocidad = Velocidad % 1.4f;
        if (!active)
        {
            if (Egg != null)
            {
                transform.position = Parabola(PosInit, PosLanzamiento, 70f, Velocidad / 1.4f);
            }
        }

        if (Velociraptor[0] == null && Velociraptor[1] == null && Velociraptor[2] == null)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                Destroy(gameObject);
            }
            else
            {
                //PhotonNetwork.Destroy(mypv);
            }
        }
        
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
        yield return new WaitForSecondsRealtime(2);
        GameObject spawv1;
        GameObject spawv2;
        GameObject spawv3;
        if (Stats.team=="Red")
        {
            
            spawv1 = PhotonNetwork.Instantiate("VelocitaptorDer", Velociraptor[0].transform.position, Quaternion.Euler(0, 180, 0));
            spawv2 = PhotonNetwork.Instantiate("VelocitaptorDer", Velociraptor[1].transform.position, Quaternion.Euler(0, 180, 0));
            spawv3 = PhotonNetwork.Instantiate("VelocitaptorDer", Velociraptor[2].transform.position, Quaternion.Euler(0, 180, 0));
        }
        else
        {
            spawv1 = PhotonNetwork.Instantiate("VelocitaptorDer", Velociraptor[0].transform.position, Quaternion.identity);
            spawv2 = PhotonNetwork.Instantiate("VelocitaptorDer", Velociraptor[1].transform.position, Quaternion.identity);
            spawv3 = PhotonNetwork.Instantiate("VelocitaptorDer", Velociraptor[2].transform.position, Quaternion.identity);
        }

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
