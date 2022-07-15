using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PhotonLobbyTest : MonoBehaviourPunCallbacks
{
    public InputField RoomNameInput;
    public Text EstadoConection;
    public Image ColorState;

    public Transform padreRoomList;
    public GameObject RoomElement;

    List<RoomInfo> myRoomList;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void GetList()
    {
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public void CreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsOpen = true;
        options.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(RoomNameInput.text, options, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("OnRoomListUpdate called");
        SetRoomList(roomList);
    }

    void SetRoomList(List<RoomInfo> roomList_)
    {
        myRoomList = roomList_;
        print("padreRoomList.childCount: " + padreRoomList.childCount);
        for (int i = padreRoomList.childCount - 1; i>=0; i--)
        {
            Destroy(padreRoomList.GetChild(i).gameObject);
        }
        print("padreRoomList.childCount: " + padreRoomList.childCount);

        for (int i = 0; i < myRoomList.Count; i++)
        {
            if(myRoomList[i].IsOpen && myRoomList[i].IsVisible && myRoomList[i].MaxPlayers > 0)
            {
                GameObject go_ = Instantiate(RoomElement, padreRoomList);
                go_.transform.GetChild(0).GetComponent<Text>().text = myRoomList[i].Name;
                go_.transform.GetChild(1).GetComponent<Text>().text = myRoomList[i].PlayerCount + "/" + myRoomList[i].MaxPlayers;

                Button btn_ = go_.GetComponent<Button>();
                string name_ = myRoomList[i].Name;

                if(myRoomList[i].PlayerCount < myRoomList[i].MaxPlayers)
                {
                    btn_.onClick.AddListener(() => { cargarRoom(name_); });
                }
                else
                {
                    go_.GetComponent<Image>().color = Color.red;
                }
                
            }
        }
    }

    public void cargarRoom(string name_)
    {
        PhotonNetwork.JoinRoom(name_);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("fallo al unirse a una room. error: " + message);
        EstadoConection.text = "Error al entrar en room";
    }

    public override void OnConnectedToMaster()
    {
        EstadoConection.text = "Conectado al servidor";
    }

    public override void OnCreatedRoom()
    {
        EstadoConection.text = "Room creada";
    }
}
