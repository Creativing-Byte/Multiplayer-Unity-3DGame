using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UserChatManager : MonoBehaviour
{
    [HideInInspector]
    public string userID, userName;

    public TextMeshProUGUI Msj, username, usernameoptions;
    public Button Add, Options, combatBtn, Report;
    public GameObject PanelOptions;
    GameObject Launcher;

    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");

        userID = userID ?? Launcher.GetComponent<UserDbInit>().DatosUser.Key.ToString();

        Options.onClick.AddListener(OnOptionsChange);
    }

    public void SendAddFriend()
    {
        Launcher.GetComponent<UserDbInit>().reloadDate();
        ChatManager ChatFriends = GetComponentInParent<ChatManager>();
        ChatFriends.SendRequestFriend(userName);
    }
    public void SendBattle()
    {
        Launcher.GetComponent<UserDbInit>().reloadDate();
        ChatManager ChatFriends = GetComponentInParent<ChatManager>();
        ChatFriends.SendRequestCombat(userName);
    }
    public void OnOptionsChange()
    {
        Launcher.GetComponent<UserDbInit>().reloadDate();
        if (PanelOptions != null)
        {
            PanelOptions.SetActive(!PanelOptions.activeSelf);
        }
    }
}
