using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Firebase.Database;
using UnityEngine.Networking;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    //----------------------------------------------------------------------------------
    string levelname;

    GameObject Player1Intanciate, Player2Intanciate;
    GameObject Player1InGame, Player2InGame;

    [HideInInspector]
    public bool AllPlayerInGameReady, StartPlayerToPlay, CreateTowerInGame;

    [HideInInspector]
    public bool WantLoadLobby = true;

    List<RoomInfo> MyRoomList;
    List<roomData> RoomList;

    public static PhotonInit PhotonInitInstance;
    public static string MyTeam = "Red";

    UnityEvent onLobbyJoined = null, onFailJoinRandom, onLeaveAnyRoom = null;

    public GameObject Pj1Offline;
    public GameObject[] TowerRedOffline;
    public GameObject[] TowerBlueOffline;

    int PlayerID = -1, IntentsConnection = 0;
    bool IAmTryConnection, FailInternet, isSwitchingRoom, createroomorfindroom;

    float timerTryConnection, timeupdatecount = 0;
    //Variables
    void Start() // 1) Funcion inicial en el flujo PhotonInit; Empieza a conectarse a internet : Ramifica en CheckInternet();
    {
        IAmTryConnection = true;
        StartCoroutine(CheckInternet());

        PhotonInitInstance = this;

        Screen.sleepTimeout = 600;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    IEnumerator CheckInternet() // 2) Funcion de conexion a internet; Verifica la conexion y los intentos de la misma : Ramifica en PhotonNetwork.ConnectUsingSettings();
    {
        while (IntentsConnection < 10 && IAmTryConnection)
        {
            UnityWebRequest uwr = UnityWebRequest.Get("https://searchconsole.googleapis.com/$discovery/rest?version=v1");//check internet
            yield return uwr.SendWebRequest();

            if (uwr.isDone)
            {
                //Debug.Log("conexion exitosa");
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.AutomaticallySyncScene = true;
                yield return new WaitForSeconds(1.5f);
                IAmTryConnection = false;
                IntentsConnection = 0;
            }
            else
            {
                yield return new WaitForSeconds(1);
                IntentsConnection++;
                Debug.Log("conexion fallida, reintentando");
            }
        }


        if (IntentsConnection >= 10)
        {
            IntentsConnection = 0;
            FailInternet = true;

            GameObject panelError = GameObject.Find("[PanelErrorNetwork]");
            if (panelError)
            {
                if (panelError.transform.childCount >= 1)
                    panelError.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    public override void OnConnectedToMaster() // 3) Funcion de inicio a la conexion con firebase; Empieza a conectarse a firebase : Ramifica en InitializeFirebase();
    {
        GetComponent<AuthInit>().InitializeFirebase();
    }
    public void CreateRoomTutorial(string level) // 8.A) Funcion para crear escena tutorial : Ramifica en LoadScene(); 
    {
        //SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        levelname = level;
        SceneManager.LoadScene(levelname);
    }
    public override void OnJoinedLobby() // 9-10) Funcion de carga de escena Lobby; Carga una Escena llamada lobby: Ramifica en CreateRoom(), LoadUnityEventWhitDelay();
    {
        if (WantLoadLobby)
        {
            CreateRoom("Lobby", true);
            WantLoadLobby = false;
        }
        else
        {
            if (onLobbyJoined != null)
            {
                StartCoroutine(LoadUnityEventWhitDelay(1.5f, onLobbyJoined));
            }
        }
    }
    public void CreateRoom(string level, bool isCustom = false, bool isForced = false) // 11) Funcion para crear una sala tanto para el lobby como salas de batalla : Finaliza 
    {
        levelname = level;

        if (level.Contains("_"))
        {
            string[] parts_ = level.Split('_');
            levelname = parts_[0];
        }

        if (isCustom)
        {
            ExitGames.Client.Photon.Hashtable h_ = new ExitGames.Client.Photon.Hashtable
            {
                { "r", levelname },
                { "d", BattleManager.DiceBet }
            };

            if (isForced)
            {
                int maxPlayer = level == "Lobby" ? 8 : 2;
                RoomOptions options_ = new RoomOptions { 
                    MaxPlayers = (byte)maxPlayer, 
                    IsVisible = true, IsOpen = true, 
                    PublishUserId = true,
                    CustomRoomProperties = h_,
                    CustomRoomPropertiesForLobby = new string[] { "r", "d" },
                    PlayerTtl = 15000
                };

                PhotonNetwork.JoinOrCreateRoom(level, options_, null);
                onFailJoinRandom = null;
            }
            else
            {
                PhotonNetwork.JoinRandomRoom(h_, 0);
                onFailJoinRandom = new UnityEvent();
                onFailJoinRandom.AddListener(() => CreateRoom(levelname, isCustom, true));
            }
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    public static IEnumerator LoadUnityEventWhitDelay(float f, UnityEvent ue_) // 10.A) Funcion para forzar la creacion de una room con Unity Events : Finaliza
    {
        yield return new WaitForSeconds(f);
        ue_.Invoke();
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "Tutorial")
        {
            string Ref1_ = "RedPoint";
            GameObject PuntoRef = GameObject.Find(Ref1_);

            if (PuntoRef != null)
            {
                for (int i = 0; i < PuntoRef.transform.childCount; i++)
                {
                    Instantiate(TowerRedOffline[i], new Vector3(PuntoRef.transform.GetChild(i).transform.position.x, 14, PuntoRef.transform.GetChild(i).transform.position.z), PuntoRef.transform.rotation);
                }
                PuntoRef = null;
            }
            string Ref2_ = "BluePoint";
            PuntoRef = GameObject.Find(Ref2_);

            if (PuntoRef != null)
            {
                for (int i = 0; i < PuntoRef.transform.childCount; i++)
                {
                    Instantiate(TowerBlueOffline[i], new Vector3(PuntoRef.transform.GetChild(i).transform.position.x, 14, PuntoRef.transform.GetChild(i).transform.position.z), PuntoRef.transform.rotation);
                }
                PuntoRef = null;
            }
            Player1Intanciate = Instantiate(Pj1Offline, new Vector3(2, 93.8f, -40), Quaternion.identity);

            if (arg0.isLoaded)
            {
                Player1Intanciate.transform.eulerAngles = new Vector3(40, 180, 0);
                Player1Intanciate.GetComponentInChildren<CardLoaded>(true).team = "Red";
                foreach (MonoBehaviour m in Pj1Offline.GetComponentsInChildren<MonoBehaviour>())
                {
                    if (!m.enabled)
                        m.enabled = true;
                }
                Player1Intanciate.SetActive(true);
                Player1Intanciate.GetComponent<Camera>().enabled = true;
                Player1Intanciate = GameObject.FindGameObjectWithTag("Camera1");
                Player1Intanciate.GetComponentInChildren<CardLoaded>().PlayGame();
                Player1Intanciate.GetComponentInChildren<CountTimerPlayer>().AssignObject();
                Player1Intanciate.GetComponent<Camera>().orthographicSize = 30;/////////////////////////
                AllPlayerInGameReady = true;
                StartPlayerToPlay = true;
            }
        }
        else
        {
            if (arg0.name != "Lobby" && arg0.name != "Tutorial")
            {
                string Ref_ = PhotonNetwork.IsMasterClient ? "RedPoint" : "BluePoint";
                GameObject PuntoRef = GameObject.FindGameObjectWithTag(Ref_);

                if (Player1Intanciate == null || Player2Intanciate == null)
                {
                    makePlayers();
                }

                if (PuntoRef)
                {
                    if (CreateTowerInGame == false)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            Debug.Log("Towers/" + arg0.name + "/TowerRedIzq");

                            PhotonNetwork.Instantiate("Towers/"+ arg0.name + "/TowerRedIzq", new Vector3(PuntoRef.transform.GetChild(0).transform.position.x, 13.76f, PuntoRef.transform.GetChild(0).transform.position.z), PuntoRef.transform.rotation);
                            PhotonNetwork.Instantiate("Towers/" + arg0.name + "/TowerRedMid", new Vector3(PuntoRef.transform.GetChild(1).transform.position.x, 14.1f, PuntoRef.transform.GetChild(1).transform.position.z), PuntoRef.transform.rotation);
                            PhotonNetwork.Instantiate("Towers/" + arg0.name + "/TowerRedDer", new Vector3(PuntoRef.transform.GetChild(2).transform.position.x, 13.76f, PuntoRef.transform.GetChild(2).transform.position.z), PuntoRef.transform.rotation);
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("Towers/" + arg0.name + "/TowerBlueIzq", new Vector3(PuntoRef.transform.GetChild(0).transform.position.x, 13.76f, PuntoRef.transform.GetChild(0).transform.position.z), PuntoRef.transform.rotation);
                            PhotonNetwork.Instantiate("Towers/" + arg0.name + "/TowerBlueMid", new Vector3(PuntoRef.transform.GetChild(1).transform.position.x, 14.1f, PuntoRef.transform.GetChild(1).transform.position.z), PuntoRef.transform.rotation);
                            PhotonNetwork.Instantiate("Towers/"+ arg0.name + "/TowerBlueDer", new Vector3(PuntoRef.transform.GetChild(2).transform.position.x, 13.76f, PuntoRef.transform.GetChild(2).transform.position.z), PuntoRef.transform.rotation);
                        }
                        CreateTowerInGame = true;
                    }
                    else
                    {
                        Debug.Log("CreateTowerInGame is true");
                    }
                }
                else
                {
                    Debug.LogError("ERROR!! no e encontro el punto de refencia " + Ref_);
                }

                if (arg0.name.ToLower().StartsWith("world"))
                {
                    StartCoroutine(Startgame());
                }
            }
            else if (arg0.name == "Lobby")
            {
                if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
                {
                    //print("se inicializa un FriendsControl");
                    CreateTowerInGame = false;
                    StartPlayerToPlay = false;
                    AllPlayerInGameReady = false;
                    GameObject friends_ = PhotonNetwork.Instantiate("FriendsControl", Vector3.zero, Quaternion.identity);
                    DontDestroyOnLoad(friends_);
                }
                else
                {
                    //print("no se esta correctamente conectado a una room de photon");
                }
            }
        }
    }
    void Update()
    {
        if (Player1InGame)
        {
            switch (SceneManager.GetActiveScene().ToString())
            {
                case "World1":
                    if (Player1InGame.GetComponent<PhotonView>().IsMine && Player1InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player1InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World2":
                    if (Player1InGame.GetComponent<PhotonView>().IsMine && Player1InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player1InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World3":
                    if (Player1InGame.GetComponent<PhotonView>().IsMine && Player1InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player1InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World4":
                    if (Player1InGame.GetComponent<PhotonView>().IsMine && Player1InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player1InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World5":
                    if (Player1InGame.GetComponent<PhotonView>().IsMine && Player1InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player1InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
            }
        }
        if (Player2InGame)
        {
            switch (SceneManager.GetActiveScene().ToString())
            {
                case "World1":
                    if (Player2InGame.GetComponent<PhotonView>().IsMine && Player2InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player2InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World2":
                    if (Player2InGame.GetComponent<PhotonView>().IsMine && Player2InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player2InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World3":
                    if (Player2InGame.GetComponent<PhotonView>().IsMine && Player2InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player2InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World4":
                    if (Player2InGame.GetComponent<PhotonView>().IsMine && Player2InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player2InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
                case "World5":
                    if (Player2InGame.GetComponent<PhotonView>().IsMine && Player2InGame.GetComponent<Camera>().orthographicSize != 80)
                    {
                        Player2InGame.GetComponent<Camera>().orthographicSize = 80;
                    }
                    break;
            }
        }

        if (IAmTryConnection || FailInternet)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name != "Lobby" && SceneManager.GetActiveScene().name != "LoadingGame")
        {
            InitializeGame();
        }

        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            timeupdatecount += Time.deltaTime;
            if (timeupdatecount >= 5)
            {
                try
                {
                    string datenow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc).ToString("MM'/'dd'/'yyyy' 'HH':'mm':'ss");
                    FirebaseDatabase.DefaultInstance.GetReference("users").Child(GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("ultcon").SetValueAsync(datenow);
                }
                catch (NullReferenceException)
                {
                }
                timeupdatecount = 0;
            }
        }
    }
    async void InitializeGame()
    {
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name != "Lobby" && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && AllPlayerInGameReady == false && StartPlayerToPlay == false)
        {
            AllPlayerInGameReady = true;
            StartPlayerToPlay = true;


            try
            {
                Player1InGame = GameObject.FindGameObjectWithTag("Camera1");
            }
            catch (NullReferenceException)
            {
            }
            try
            {
                Player2InGame = GameObject.FindGameObjectWithTag("Camera2");
            }
            catch (NullReferenceException)
            {

            }

            if (BattleManager.DiceBet > 0)
            {
                var diceRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("dice");
                int dice = int.Parse((await diceRef.GetValueAsync()).Value.ToString());
                await diceRef.SetValueAsync(dice - BattleManager.DiceBet);
            }
            StartCoroutine(Startgame());
        }
    }
    public void TryAgainConnection()
    {
        GameObject panelError = GameObject.Find("[PanelErrorNetwork]");
        if (panelError)
        {
            if (panelError.transform.childCount >= 1)
                panelError.transform.GetChild(0).gameObject.SetActive(false);
        }

        StartCoroutine(CheckInternet());
    }
    IEnumerator Startgame()
    {
        yield return new WaitForSeconds(5);
        try
        {
            if (Player1Intanciate)
            {
                if (Player1Intanciate.GetComponent<PhotonView>().IsMine)
                {
                    Player1Intanciate.SetActive(true);
                    switch (SceneManager.GetActiveScene().ToString())
                    {
                        case "World1":
                            Player1Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                        case "World2":
                            Player1Intanciate.transform.localPosition = new Vector3(1, 198, 225);
                            //Player1Intanciate.transform.localPosition = new Vector3(1, 198, 225);
                            Player1Intanciate.transform.localEulerAngles = new Vector3(65, 180, 0);
                            Player1Intanciate.GetComponent<Camera>().orthographicSize = 80;
                            break;
                        case "World3":
                            Player1Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                        case "World4":
                            Player1Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                        case "World5":
                            Player1Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                    }
                    Player1Intanciate.GetComponentInChildren<CardLoaded>().PlayGame();
                    Player1Intanciate.GetComponentInChildren<CountTimerPlayer>().AssignObject();
                }
            }

        }
        catch (NullReferenceException)
        {
        }
        catch (UnassignedReferenceException)
        {
        }

        try
        {
            if (Player2Intanciate)
            {
                if (Player2Intanciate.GetComponent<PhotonView>().IsMine)
                {
                    Player2Intanciate.SetActive(true);
                    switch (SceneManager.GetActiveScene().ToString())
                    {
                        case "World1":
                            Player2Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                        case "World2":
                            Player1Intanciate.transform.localPosition = new Vector3(1, 198, 112.5f);
                            Player1Intanciate.transform.localEulerAngles = new Vector3(65, 0, 0);
                            Player2Intanciate.GetComponent<Camera>().orthographicSize = 80;
                            break;
                        case "World3":
                            Player2Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                        case "World4":
                            Player2Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                        case "World5":
                            Player2Intanciate.GetComponent<Camera>().orthographicSize = 30;
                            break;
                    }
                    Player2Intanciate.GetComponentInChildren<FadePanel>().pv.RPC("FadeOnline", RpcTarget.All);
                    Player2Intanciate.GetComponentInChildren<CardLoaded>().PlayGame();
                    Player2Intanciate.GetComponentInChildren<CountTimerPlayer>().AssignObject();
                }
            }
        }
        catch (NullReferenceException)
        {
        }
        catch (UnassignedReferenceException)
        {
        }
        yield return null;
    }
    public void switchToRoom(string level, bool isMatchMaking = true)
    {
        if (isSwitchingRoom)
            return;

        levelname = level;

        if (RoomList == null)
        {
            RoomList = new List<roomData>();
        }
        StartCoroutine(SwitchingScene(level, isMatchMaking));
    }
    IEnumerator SwitchingScene(string level_, bool isMatchMaking = true)
    {
        isSwitchingRoom = true;

        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();

        int intents = 0;

        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        while (!PhotonNetwork.IsConnectedAndReady && intents < 5)
        {
            yield return new WaitForSeconds(1.5f);
            if (PhotonNetwork.NetworkClientState == ClientState.Disconnected)
            {
                Debug.Log("Conectado");
                PhotonNetwork.ConnectUsingSettings();
            }
            PhotonNetwork.AutomaticallySyncScene = true;
            intents++;
        }

        if (intents >= 5)
        {
            Debug.LogError("hey no estas conectado a photon!!");
        }

        intents = 0;

        while (!PhotonNetwork.InLobby && intents < 5)
        {
            yield return new WaitForSeconds(1.5f);

            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable {
                    { "UserId", GameObject.FindGameObjectWithTag("Launcher").GetComponent<UserDbInit>().DatosUser.Key }
                });
                PhotonNetwork.JoinLobby();
                yield return new WaitForSeconds(2.5f);
                intents = 100;
            }
            intents++;
        }

        intents = 0;

        while (!PhotonNetwork.IsConnectedAndReady && intents < 5)
        {
            print("esperando estado a que este listo");
            yield return new WaitForSeconds(2.5f);
            intents++;

            if (intents >= 5)
            {
                print("estado nunca fue listo. estado actual:" + PhotonNetwork.NetworkClientState.ToString());
            }
        }

        if (MyRoomList == null)
            MyRoomList = new List<RoomInfo>();


        string roomname = level_ == "Lobby" ? level_ : level_ + "_" + UnityEngine.Random.Range(9999, 99999);

        if (isMatchMaking)
        {
            if (level_ != "Lobby")
            {
                if (MyRoomList != null)
                {
                    for (int i = 0; i < MyRoomList.Count; i++)
                    {
                        if (MyRoomList[i].Name.StartsWith(level_))
                        {
                            if (MyRoomList[i].MaxPlayers > 0)
                            {
                                if (MyRoomList[i].PlayerCount < MyRoomList[i].MaxPlayers && int.Parse(MyRoomList[i].CustomProperties["d"].ToString()) == BattleManager.DiceBet)
                                {
                                    roomname = MyRoomList[i].Name;
                                    print("roomname finded. new roomname is: " + roomname);
                                    createroomorfindroom = true;
                                    break;
                                }
                            }
                            else
                            {
                                roomname = MyRoomList[i].Name;
                                print("roomname finded. new roomname is: " + roomname);
                                break;
                            }

                        }
                    }
                }
            }
        }
        else
        {
            roomname = level_;
        }

        CreateRoom(roomname, true, true);

        isSwitchingRoom = false;
    }

    public void ExitApp()
    {
        if (Application.isEditor)
        {
            Debug.Break();
        }
        else
        {
            Application.Quit();
        }
    }
    public void makePlayers()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            AudioListener al_ = FindObjectOfType<AudioListener>();
            if (al_)
                al_.enabled = false;


            if (SceneManager.GetActiveScene().name != levelname)
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.LoadLevel(levelname);

            if (PlayerID == 1)
            {
                Player1Intanciate = PhotonNetwork.Instantiate("P1", new Vector3(2, 93.8f, 45), Quaternion.identity);//40
                Player1Intanciate.transform.eulerAngles = new Vector3(61.6f, 180, 0);
                Player1Intanciate.GetComponentInChildren<CardLoaded>(true).team = "Red";

                foreach (MonoBehaviour m in Player1Intanciate.GetComponentsInChildren<MonoBehaviour>())
                {
                    if (!m.enabled)
                        m.enabled = true;
                }

                Player1Intanciate.GetComponent<Camera>().enabled = true;
                MyTeam = "Red";
            }

            if (PlayerID == 2)
            {
                Player2Intanciate = PhotonNetwork.Instantiate("P2", new Vector3(2, 93.8f, -45), Quaternion.identity);//-40
                Player2Intanciate.transform.eulerAngles = new Vector3(61.6f, 0, 0);
                Player2Intanciate.GetComponentInChildren<CardLoaded>(true).team = "Blue";

                foreach (MonoBehaviour m in Player2Intanciate.GetComponents<MonoBehaviour>())
                {
                    if (!m.enabled)
                        m.enabled = true;
                }
                Player2Intanciate.GetComponent<Camera>().enabled = true;
                MyTeam = "Blue";
                Player2Intanciate.GetComponentInChildren<CardLoaded>().SendDataBattleFunct();
            }
        }
        else
        {
            print("no hay 1 usuario en la sala, actualmente hay: " + PhotonNetwork.CountOfPlayersInRooms);
        }
    }
    //----------------------------------------------------------------------------------
    public override void OnJoinedRoom()
    {
        string myRoom = PhotonNetwork.CurrentRoom.Name;

        //Debug.Log("JoinedRoom sucess:" + myRoom);

        DataManager lbControl = FindObjectOfType<DataManager>();
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            if (!createroomorfindroom)
            {
                lbControl.TextLoading.text = "Room create, wait Oponents";
            }
            else
            {
                lbControl.TextLoading.text = "Room found, joining";
            }
        }
        createroomorfindroom = false;

        bool isReadyToPlay = false;

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("RoomActual"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Remove("RoomActual");
            PhotonNetwork.LocalPlayer.CustomProperties.Add("RoomActual", myRoom);
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("RoomActual", myRoom);
        }

        if (myRoom == "Lobby")
        {
            LobbyControl lobbyControl = FindObjectOfType<LobbyControl>();
            if (lobbyControl)
            {
                lobbyControl.DesactivarPanelLoading();
            }
        }

        if (levelname != "Lobby")
        {
            PlayerID = PhotonNetwork.PlayerList.Length;
        }
        else
        {
            if (SceneManager.GetActiveScene().name != levelname)
                PhotonNetwork.LoadLevel(levelname);
        }

        if (isReadyToPlay)
        {
            AudioListener al_ = FindObjectOfType<AudioListener>();
            if (al_)
                al_.enabled = false;

            if (PlayerID == 2)
            {
                Player2Intanciate = PhotonNetwork.Instantiate("P" + PlayerID, new Vector3(2.41f, 195f, -279f), Quaternion.identity);
                Player2Intanciate.transform.eulerAngles = new Vector3(40, 0, 0);
                Player2Intanciate.GetComponentInChildren<CardLoaded>(true).team = "Blue";

                foreach (MonoBehaviour m in Player2Intanciate.GetComponents<MonoBehaviour>())
                {
                    if (!m.enabled)
                        m.enabled = true;
                }
                Player2Intanciate.GetComponent<Camera>().enabled = true;
                MyTeam = "Blue";
            }
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (onFailJoinRandom != null)
        {
            onFailJoinRandom.Invoke();
        }
        else
        {
            Debug.Log("Failed to found room");
            DataManager lbControl = FindObjectOfType<DataManager>();
            lbControl.TextLoading.text = "Room not found... Creating Room";
            createroomorfindroom = true;
            PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), new RoomOptions { MaxPlayers = 2 }, null);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        MyRoomList = roomList;
    }
    public override void OnLeftRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (onLeaveAnyRoom != null)
            {
                StartCoroutine(LoadUnityEventWhitDelay(1.5f, onLeaveAnyRoom));
            }
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player entered: " + newPlayer.UserId);

        if (levelname != "Lobby")
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                DataManager lbControl = FindObjectOfType<DataManager>();
                lbControl.TextLoading.text = "Oponents found, load level";

                AudioListener al_ = FindObjectOfType<AudioListener>();
                if (al_)
                    al_.enabled = false;

                if (SceneManager.GetActiveScene().name != levelname)
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.LoadLevel(levelname);
            }
            else
            {
                print("no hay 1 usuario en la sala, actualmente hay: " + PhotonNetwork.CountOfPlayersInRooms);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // Handle other player left
        base.OnPlayerLeftRoom(otherPlayer);
    }
    //Calbacks
}
public class roomData
{
    public string name;
    public int players;
    public int maxPlayers;
}
