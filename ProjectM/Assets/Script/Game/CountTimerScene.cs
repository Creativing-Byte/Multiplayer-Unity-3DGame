using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountTimerScene : MonoBehaviour
{
    public string TimerToText;
    int TimerM, TimerS;
    float TimerSF;
    bool start = false;
    GameObject Launcher;

    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
    }
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            if (!start)
            {
                TimerM = 5;
                TimerSF = 0f;
                TimerToText = TimerM + ":" + TimerS;
                ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
                setValue.Add("time", TimerToText);
                setValue.Add("M", TimerM);
                setValue.Add("S", TimerS);

                if (PhotonNetwork.LocalPlayer.IsMasterClient)       
                    PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
                //else
                //{
                //    TimerToText = PhotonNetwork.CurrentRoom.CustomProperties["time"].ToString();
                //    TimerM = (int)PhotonNetwork.CurrentRoom.CustomProperties["M"];
                //    TimerS = (int)PhotonNetwork.CurrentRoom.CustomProperties["S"];
                //    RedPoint = (int)PhotonNetwork.CurrentRoom.CustomProperties["Red"];
                //    BluePoint = (int)PhotonNetwork.CurrentRoom.CustomProperties["Blue"];
                //}

                start = true;
            }
        }
    }

    void Update()
    {
        if (start)
        {
            if (TimerSF <= 0)
            {
                ControlTime();
                TimerM -= 1;
                TimerSF = 60f;
            }
            TimerSF -= Time.deltaTime;
            TimerS = (int)TimerSF;

            if (TimerSF > 10)
            {
                TimerToText = TimerM + ":" + TimerS;
            }
            else
            {
                TimerToText = TimerM + ":0" + TimerS;
            }
        }
    }
    void ControlTime()
    {
        if (TimerM <= 0 && TimerS <= 0)
        {
            BattleManager.instance.EndGame();
        }
    }
}
