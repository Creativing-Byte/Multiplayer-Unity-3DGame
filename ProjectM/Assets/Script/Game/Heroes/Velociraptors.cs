using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Velociraptors : MonoBehaviour
{
    [HideInInspector]
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
                PhotonNetwork.Destroy(mypv);
            }
        }
    }
    public void Active()
    {
        for (int i = 0; i < Velociraptor.Length; i++)
        {
            if (Velociraptor[i].activeInHierarchy == false)
            {
                Velociraptor[i].SetActive(true);
            }
        }
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            Destroy(Egg);
        }
        else
        {
            PhotonNetwork.Destroy(Egg);
        }
        active = true;
    }
    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
