using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsManager : MonoBehaviour
{
    public GameObject FriendPrefrab;
    GameObject Launcher;
    [HideInInspector]
    public List<string> Friends = new List<string>();
    List<Friend> FriendsLoaded = new List<Friend>();
    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        LoadFriends();
    }
    public void LoadFriends()
    {
        StartCoroutine(loadfriends());
    }
    IEnumerator loadfriends()
    {
        try
        {
            Friends.Clear();
        }
        catch (NullReferenceException)
        {
        }
        try
        {
            FriendsLoaded.Clear();
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
                    Friends.Add(Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("friends").Child(i.ToString()).Value.ToString());
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < Friends.Count; i++)
            {
                FriendsLoaded.Insert(i, (Launcher.GetComponent<UserDbInit>().LoadFriendData(Friends[i])));
                GameObject FriendPrebas = Instantiate(FriendPrefrab, new Vector3(0, 0, 0), Quaternion.identity);
                FriendPrebas.gameObject.transform.SetParent(gameObject.transform);
                FriendPrebas.GetComponent<FriendPrefab>().f = Launcher.GetComponent<UserDbInit>().LoadFriendData(Friends[i]);
                FriendPrebas.GetComponent<FriendPrefab>().f.Id = Friends[i];
            }
        }
        catch (NullReferenceException)
        {
        }
        yield return null;
    }
}
