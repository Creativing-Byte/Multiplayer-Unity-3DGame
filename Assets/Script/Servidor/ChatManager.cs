using System;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Firebase.Database;
using Photon.Chat;
using Photon.Pun;
using System.Threading.Tasks;
using System.Collections;

public class ChatManager : MonoBehaviourPun, IChatClientListener
{
    ChatClient clientChat;
    public static ChatClient GetclientChat;

    public string[] ChannelsToJoinOnConnect;

    public TMPro.TMP_InputField InputFieldChat;
    public GameObject MsjPrefabMine, MsjPrefabOther, PanelAddFriends, PanelCombat;
    public Text StateText;

    public int HistoryLengthToFetch;
    readonly string selectedChannelName;

    public int TestLength = 2048;
    private readonly byte[] testBytes = new byte[2048];

    GameObject Launcher;

    public List<string> friends = new List<string>();

    [HideInInspector]
    public static string USERID;
    [HideInInspector]
    public string userRequestID;
    [HideInInspector]
    public string SolicitanteKey;

    public GameObject FriendsController;

    public Button SendMsj;
    UserChatManager ucm;
    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        clientChat = new ChatClient(this);
        GetclientChat = clientChat;

        USERID = Launcher.GetComponent<UserDbInit>().DatosUser.Key;

        Connect();
    }
    void Update()
    {
        clientChat.Service();
    }
    void Connect()
    {
        try
        {
            clientChat.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new Photon.Chat.AuthenticationValues(Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString()));
        }
        catch (NullReferenceException)
        {
            print("se esta conectando sin el launcher");
            clientChat.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(Guid.NewGuid().ToString()));
        }
    }
    void Disconnect()
    {
        clientChat.Disconnect();
        Connect();
    }
    public void OnClickSend()
    {
        if (!string.IsNullOrEmpty(InputFieldChat.text))
        {
            SendChatMessage(InputFieldChat.text);
            InputFieldChat.text = "";
        }
    }
    private void SendChatMessage(string inputLine)
    {
        clientChat.PublishMessage("All",/* clientChat.UserId + " : " + */inputLine);
    }
    public void ShowNewMsj(string channelName, string mensaje, string emisor_ = "")
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;

        bool found = clientChat.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        bool isMineMsg = emisor_ == Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString();
        GameObject Msj = Instantiate(isMineMsg ? MsjPrefabMine : MsjPrefabOther, gameObject.transform);
        ucm = Msj.GetComponent<UserChatManager>();
        Msj.transform.localScale = new Vector3(1, 1, 1);
        ucm.userName = emisor_;
        StartCoroutine(MoveChatToBottom());
        if (!string.IsNullOrEmpty(channel.ToStringMessages()))
        {
            Launcher.GetComponent<UserDbInit>().reloadDate();
            if (emisor_ == Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString())
            {
                ucm.userID = Launcher.GetComponent<UserDbInit>().DatosUser.Key;
                Msj.GetComponent<UserChatManager>().Msj.text = mensaje;
            }
            else
            {
                Msj.GetComponent<UserChatManager>().Msj.text = mensaje;
                ucm.username.text = emisor_.ToUpper();
                ucm.usernameoptions.text = emisor_.ToUpper();
            }

            friends.Clear();
            FriendsManager friendmanager = FindObjectOfType<FriendsManager>();

            for (int i = 0; i < friendmanager.Friends.Count; i++)
            {
                string username = "";
                _ = FirebaseDatabase.DefaultInstance
                  .GetReference("users").Child(friendmanager.Friends[i])
                  .GetValueAsync().ContinueWith(task =>
                  {
                      if (task.IsFaulted)
                      {
                      }
                      if (task.IsCompleted)
                      {
                          username = task.Result.Child("Date").Child("username").Value.ToString();

                          if (emisor_ != Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString())
                          {
                              if (emisor_ == username)
                              {
                                  Debug.Log("Son amigos " + username);
                                  ucm.Add.interactable = false;
                              }
                              else
                              {
                                  Debug.Log("No son amigos " + username);
                              }
                          }
                          else
                          {
                              Debug.Log("Soy yo");
                          }
                      }
                  });
            }
        }
    }

    IEnumerator MoveChatToBottom()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void SendRequestFriend(string id)
    {
        print("SendRequestFriend was called");

        FriendsControl[] fcs = FindObjectsOfType<FriendsControl>();
        FriendsControl fc = null;
        for (int i = 0; i < fcs.Length; i++)
        {
            if (fcs[i].photonView.IsMine)
            {
                fc = fcs[i];
            }
        }

        if (fc)
        {
            PhotonView pv_ = fc.GetComponent<PhotonView>();
            if (pv_)
            {
                string content_ = id + "|" + clientChat.UserId + "|" + USERID;
                Debug.Log("calling RequestFriendRPC");
                fc.requestFriend(content_);
            }
            else
            {
                Debug.LogError("FriendsControl have not PhotoView");
            }
        }
        else
        {
            if (PhotonNetwork.InRoom)
            {
                GameObject friends_ = PhotonNetwork.Instantiate("FriendsControl", Vector3.zero, Quaternion.identity);

                if (friends_)
                    DontDestroyOnLoad(friends_);
            }
        }
    }
    public void SendRequestCombat(string id)
    {
        print("SendRequestCombat was called");
        Debug.Log(id);

        FriendsControl[] fcs = FindObjectsOfType<FriendsControl>();
        FriendsControl fc = null;
        for (int i = 0; i < fcs.Length; i++)
        {
            if (fcs[i].photonView.IsMine)
            {
                fc = fcs[i];
            }
        }

        if (fc)
        {
            PhotonView pv_ = fc.GetComponent<PhotonView>();
            if (pv_)
            {
                string content_ = id + "|" + clientChat.UserId + "|" + USERID;
                Debug.Log(content_);
                fc.requestCombat(content_);
            }
            else
            {
                Debug.LogError("FriendsControl have not PhotoView");
            }
        }
        else
        {
            if (PhotonNetwork.InRoom)
            {
                GameObject friends_ = PhotonNetwork.Instantiate("FriendsControl", Vector3.zero, Quaternion.identity);

                if (friends_)
                    DontDestroyOnLoad(friends_);
            }
        }
    }   //solicita un combate a un jugador
    public void ResponderInvitacion(bool b_)
    {
        FriendsControl[] fcs = FindObjectsOfType<FriendsControl>();
        FriendsControl fc = null;
        for (int i = 0; i < fcs.Length; i++)
        {
            if (fcs[i].photonView.IsMine)
            {
                fc = fcs[i];
            }
        }

        if (fc)
        {
            PhotonView pv_ = fc.GetComponent<PhotonView>();
            if (pv_)
            {
                fc.ResponseRequest(userRequestID, clientChat.UserId, b_);


                if (b_)
                {
                    AddFriend(SolicitanteKey);
                    //lo agregamos a la lista de amigos mios
                }
                ManageStatusPanelRequest(false);
            }
        }
    }
    public void ResponderInvitacionCombate(bool b_)
    {
        FriendsControl[] fcs = FindObjectsOfType<FriendsControl>();
        FriendsControl fc = null;
        for (int i = 0; i < fcs.Length; i++)
        {
            if (fcs[i].photonView.IsMine)
            {
                fc = fcs[i];
            }
        }

        if (fc)
        {
            PhotonView pv_ = fc.GetComponent<PhotonView>();
            if (pv_)
            {
                fc.ResponseRequestCombat(userRequestID, clientChat.UserId, b_);


                if (b_)
                {
                    LobbyControl lc = FindObjectOfType<LobbyControl>();
                    if (lc)
                    {
                        lc.ActivarPanelLoading();
                    }

                    Launcher.GetComponent<DeckManager>().StartGame(true);
                    string ourRoom = "World1_" + userRequestID + "-" + clientChat.UserId;    // change this later
                    PhotonInit.PhotonInitInstance.switchToRoom(ourRoom, false);
                }
                ManageStatusPanelCombat(false);
            }
        }
    }
    public void AddFriend(string friendID)
    {
        GameObject Launcher;
        Launcher = GameObject.FindGameObjectWithTag("Launcher");

        Launcher.GetComponent<UserDbInit>().reloadDate();

        string key = Launcher.GetComponent<UserDbInit>().DatosUser.Key;

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();

        int friendnum = int.Parse(Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("friends").ChildrenCount.ToString());
        childUpdates[friendnum.ToString()] = friendID;

        FirebaseDatabase.DefaultInstance.GetReference("users").Child(Launcher.GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("friends").UpdateChildrenAsync(childUpdates).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                FirebaseDatabase.DefaultInstance.GetReference("users").Child(Launcher.GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("friends").UpdateChildrenAsync(childUpdates);
            }
            if (task.IsCompleted)
            {
                Debug.Log("Añadido con exito");
                FriendsController.GetComponent<FriendsManager>().LoadFriends();
            }
        });

        Launcher.GetComponent<UserDbInit>().reloadDate();
    }
    public void ManageStatusPanelRequest(bool b_, string invitador = "")
    {
        if (PanelAddFriends)
        {
            PanelAddFriends.SetActive(b_);

            string invitacion_ = string.Format("{0} te ha enviado una solicitud de amistad. ¿deseas aceptar?", invitador);
            PanelAddFriends.transform.GetChild(1).GetComponent<Text>().text = invitacion_;
        }
    }
    public void ManageStatusPanelCombat(bool b_, string invitador = "")
    {
        if (PanelCombat)
        {
            PanelCombat.SetActive(b_);

            string invitacion_ = string.Format("{0} te ha enviado una solicitud de combate. ¿deseas aceptar?", invitador);
            PanelCombat.transform.GetChild(1).GetComponent<Text>().text = invitacion_;
        }
    }
    public void SendReportFriend(string id)
    {
        //pendiente
    }

    //--------- callbacks ----------------------
    #region CallBacks
    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            //Debug.Log(message);
        }
    }
    public void OnChatStateChange(ChatState state)
    {
        StateText.text = state.ToString();
        if (state == ChatState.ConnectedToFrontEnd)
        {
            SendMsj.interactable = true;
        }
        else
        {
            SendMsj.interactable = false;
        }
    }
    public void OnConnected()
    {
        if (ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
        {
            clientChat.Subscribe(ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
        }

        clientChat.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
    }
    public void OnDisconnected()
    {
        Connect();
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //print("OnGetMessages was called");

        for (int i = 0; i < senders.Length; i++)
        {
            //Debug.LogFormat("sender[{0}]:{1}", i, senders[i]);
        }

        for (int i = 0; i < messages.Length; i++)
        {
            ShowNewMsj("All", messages[i].ToString(), senders[i]);
        }
    }
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new NotImplementedException();
    }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new NotImplementedException();
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (string channel in channels)
        {
            //clientChat.PublishMessage(channel, clientChat.UserId + " Join to channel");
        }
    }
    public void OnUnsubscribed(string[] channels)
    {
        if (ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
        {
            clientChat.Subscribe(ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
        }

        clientChat.SetOnlineStatus(ChatUserStatus.Online);
    }
    public void OnUserSubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }
    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new NotImplementedException();
    }
    #endregion
}
