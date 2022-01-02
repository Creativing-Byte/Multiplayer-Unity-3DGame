using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FriendsControl : MonoBehaviourPun
{
    [HideInInspector]
    public string Myinvitador, SolicitanteKey;
    GameObject Launcher;

    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
    }
    public void requestFriend(string content_)
    {
        photonView.RPC("RequestFriendRPC", RpcTarget.All, content_);
    }
    [PunRPC]
    public void RequestFriendRPC(string content_)
    {
        Debug.Log(content_);
        string[] contenido = content_.Split('|');
        string invitadousername = contenido[0];
        string invitadorusername = contenido[1];
        string invitadorUID = contenido[2];
        bool amigosbool = false;

        string myUserId = Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString();
        if (invitadousername == myUserId)
        {
            List<string> amigos = Launcher.GetComponent<UserDbInit>().LoadFriendList();
            for (int i = 0; i < amigos.Count; i++)
            {
                Debug.Log(invitadorUID + " " + amigos[i]);
                if (invitadorUID == amigos[i])
                {
                    amigosbool = true;
                }
            }

            if (!amigosbool)
            {
                ChatManager cm_ = FindObjectOfType<ChatManager>();
                if (cm_)
                {
                    cm_.ManageStatusPanelRequest(true, invitadorusername);
                    cm_.userRequestID = invitadorusername;
                    cm_.SolicitanteKey = invitadorUID;
                }
            }
        }
    }
    public void ResponseRequest(string invitador, string remitente, bool response_)
    {
        string content = invitador + "|" + remitente + "|" + ChatManager.USERID;

        photonView.RPC("ResponseRequestRPC", RpcTarget.Others, content, response_);
    }
    [PunRPC]
    public void ResponseRequestRPC(string content_, bool response_)
    {
        if (Launcher == null)
            Launcher = GameObject.FindGameObjectWithTag("Launcher");

        print("ResponseRequestRPC was called, content: " + content_);

        string[] contenido = content_.Split('|');
        string invitador = contenido[0];
        string remitente = contenido[1];

        if (invitador == remitente)
        {
            return;
        }

        string myUserId = Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString();

        string textResponse = response_ ? "acepto" : "rechazo";
        Debug.LogFormat("el usuario {0} {1} la invitacion enviada por {2}, tu userid es {3}", remitente, textResponse , invitador, myUserId);

        if (myUserId == invitador)
        {
            ChatManager cm_ = FindObjectOfType<ChatManager>();
            if (cm_)
            {
                if (response_)
                {
                    print("amigo agregado " + remitente);
                    cm_.AddFriend(contenido[2]);
                }
            }
        }

        if (!response_)
        { 
            print("amigo no agregado");
        }
    }
    public void requestCombat(string content_)
    {
        photonView.RPC("requestCombatRPC", RpcTarget.All, content_);
    }
    [PunRPC]
    public void requestCombatRPC(string content_)
    {
        if (Launcher == null)
            Launcher = GameObject.FindGameObjectWithTag("Launcher");

        print(content_);
        string[] contenido = content_.Split('|');
        string invitado = contenido[0];
        string invitador = contenido[1];

        

        string myUserId = Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString();
        if (invitado == myUserId)
        {
            ChatManager cm_ = FindObjectOfType<ChatManager>();
            if (cm_)
            {
                cm_.ManageStatusPanelCombat(true, invitador);
                cm_.userRequestID = invitador;
                cm_.SolicitanteKey = contenido[2];
            }
        }

        //Debug.LogFormat("{0} was send a friend request to {1}. you are {2}", invitador, invitado, myUserId);
    }
    public void ResponseRequestCombat(string invitador, string remitente, bool response_)
    {
        if (Launcher == null)
            Launcher = GameObject.FindGameObjectWithTag("Launcher");

        string content = invitador + "|" + remitente + "|" + ChatManager.USERID;

        photonView.RPC("ResponseRequestCombatRPC", RpcTarget.Others, content, response_);
    }
    [PunRPC]
    public void ResponseRequestCombatRPC(string content_, bool response_)
    {
        if (Launcher == null)
            Launcher = GameObject.FindGameObjectWithTag("Launcher");

        print("ResponseRequestRPC was called, content: " + content_);

        string[] contenido = content_.Split('|');
        string invitador = contenido[0];
        string remitente = contenido[1];

        if (invitador == remitente)
        {
            return;
        }

        string myUserId = Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("username").Value.ToString();

        string textResponse = response_ ? "acepto" : "rechazo";
        Debug.LogFormat("el usuario {0} {1} la invitacion  de combate enviada por {2}, tu userid es {3}", remitente, textResponse, invitador, myUserId);

        if (myUserId == invitador)
        {
            ChatManager cm_ = FindObjectOfType<ChatManager>();
            if (cm_)
            {
                if (response_)
                {
                    LobbyControl lc = FindObjectOfType<LobbyControl>();
                    if (lc)
                    {
                        lc.ActivarPanelLoading();
                    }

                    Launcher.GetComponent<DeckManager>().StartGame(true);
                    string ourRoom = "World1_" + myUserId + "-" + remitente;    // change this later
                    PhotonInit.PhotonInitInstance.switchToRoom(ourRoom, false);
                }
            }
        }

        if (!response_)
        {
            print("amigo no agregado");
        }
    }
}
