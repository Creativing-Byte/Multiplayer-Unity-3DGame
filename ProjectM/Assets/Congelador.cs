using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Congelador : MonoBehaviour
{
    public Player playerScript;
    public NavMeshAgent myBrain;
    public Rigidbody playerRB;
    void Start()
    {
        playerScript = GetComponent<Player>();
        myBrain = GetComponent<NavMeshAgent>();
        playerRB = GetComponent<Rigidbody>();
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
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;

    }
}
