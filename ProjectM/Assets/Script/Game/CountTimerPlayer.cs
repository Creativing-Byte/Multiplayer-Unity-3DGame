using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CountTimerPlayer : MonoBehaviour
{
    public TextMeshProUGUI Timer, Red,Blue;
    bool Play;
    GameObject CountTimerScene;
    public void AssignObject()
    {
        CountTimerScene = GameObject.FindGameObjectWithTag("Count");
        CountTimerScene.GetComponent<CountTimerScene>().StartGame();
        Play = true;
    }
    void Update()
    {
        if (Play)
        {
            Red.text = BattleManager.instance.redPoints.ToString();
            Blue.text = BattleManager.instance.bluePoints.ToString();
            Timer.text = CountTimerScene.GetComponent<CountTimerScene>().TimerToText;
        }
    }
}
