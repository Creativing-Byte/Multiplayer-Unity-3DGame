using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public GameObject PlayerPrefab;
    GameObject Launcher;
    [HideInInspector]
    public List<string> PlayerTop = new List<string>();
    List<Friend> PlayersLoad = new List<Friend>();
    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        LoadLeader();
    }
    public void LoadLeader()
    {
        StartCoroutine(Leader());
    }
    IEnumerator Leader()
    {
        try
        {
            PlayerTop.Clear();
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            PlayersLoad.Clear();
        }
        catch (NullReferenceException)
        {

        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Launcher.GetComponent<UserDbInit>().reloadDate();

        yield return new WaitForSeconds(3);

        try
        {
            for (int i = 0; i < Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("friends").ChildrenCount; i++)
            {
                if (!string.IsNullOrEmpty(Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("friends").Child(i.ToString()).Value.ToString()))
                {
                    PlayerTop.Add(Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("friends").Child(i.ToString()).Value.ToString());
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < PlayerTop.Count; i++)
            {
                PlayersLoad.Insert(i, (Launcher.GetComponent<UserDbInit>().LoadFriendData(PlayerTop[i])));
            }
            PlayersLoad.OrderBy(x => x.destreza);
            for (int i = 0; i < PlayersLoad.Count; i++)
            {
                GameObject Player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                Player.gameObject.transform.SetParent(gameObject.transform);
                Player.GetComponent<FriendPrefab>().f = PlayersLoad[i];
                Player.GetComponent<FriendPrefab>().f.Id = PlayersLoad[i].Id;
            }
        }
        catch (NullReferenceException)
        {
        }
        yield return null;
    }
}
