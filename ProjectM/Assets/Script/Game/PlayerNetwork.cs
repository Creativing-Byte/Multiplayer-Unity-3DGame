using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerNetwork : MonoBehaviour
{
    public GameObject[] componentesObservables;
    PhotonView myview;
    // Start is called before the first frame update
    void Awake()
    {
        Camera mycamara = GetComponent<Camera>();
        myview = GetComponent<PhotonView>();
        if (!myview.IsMine)
        {
            try
            {
                for (int i = 0; i < componentesObservables.Length; i++)
                {
                    componentesObservables[i].SetActive(false);
                }
            }
            catch (UnassignedReferenceException)
            {
            }

            if (mycamara)
            {
                mycamara.enabled = false;
            }
        }
        else
        {
            try
            {
                for (int i = 0; i < componentesObservables.Length; i++)
                {
                    componentesObservables[i].SetActive(true);
                }
            }
            catch (UnassignedReferenceException)
            {
            }

            if (mycamara)
            {
                mycamara.enabled = true;
            }
        }
    }

    public bool isMine()
    {
        if (myview)
        {
            string mineText = myview.IsMine ? "mine" : "of other player";
            return myview.IsMine;
        }

        return true;
    }
}
