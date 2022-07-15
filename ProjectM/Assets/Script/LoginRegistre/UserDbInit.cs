using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserDbInit : MonoBehaviour
{
    public DataSnapshot DatosUser;
    FirebaseUser userp;

    public delegate void OnDataReloaded();
    public static event OnDataReloaded onDataReloadedComplete;

    public bool LobbyTest, TutorialTest, EscenaTest, worldOne;
    public void InicializarBd(FirebaseUser user) // 6) Funcion inicial en el flujo UserDbInit; Configura y verifica la conexion al servicio de RealtimeDatabase : Ramifica en DownloadDataSnapshotUser();
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //una vez iniciada la base de datos cargamos los datos del usuario
                //Debug.Log("Verifi dependencis Ok!");
                DownloadDataSnapshotUser(user);
                userp = user;
            }
            else
            {
                InicializarBd(user);
            }
        });
    }
    void DownloadDataSnapshotUser(FirebaseUser user) // 7) Funcion de carga de datos del usuario; Baja un DataSnapshot de los datos del usuario : Ramifica en ReadDataSceneUser();
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(user.UserId)
      .GetValueAsync().ContinueWith(async task =>
      {
          if (task.IsFaulted)
          {
              DownloadDataSnapshotUser(user);
          }
          if (task.IsCompleted)
          {
              Debug.Log("Date User Download");
              ReadDataSceneUser(await task);
          }
      });
    }
    public async void ReadDataSceneUser(DataSnapshot datos) // 8) Funcion de lectura de los datos del usuario; Lee el valor guardado en el campo Scene para verificar si es usuario nuevo o regular : 9 Ramifica en JoinLobby(), 8.A CreateRoomTutorial();
    {
        //await Task.Delay(5000);
        DatosUser = datos;
        Debug.Log("ReadDataSceneUser");
        //new WaitForSecondsRealtime(5f);
        if (LobbyTest)
        {
            Debug.Log("Loading Lobby");
            PhotonNetwork.JoinLobby();
        }
        else if (TutorialTest)
        {
            Debug.Log("Loading Tutorial");
            GetComponent<DeckManager>().StartGame(false);
            GetComponent<PhotonInit>().CreateRoomTutorial("Tutorial");
        }
        else if (EscenaTest)
        {
            GetComponent<PhotonInit>().CreateRoomTutorial("Escenario de testing");
        }
        else if (worldOne)
        {
            GetComponent<PhotonInit>().CreateRoomTutorial("World1");
        }
        else
        {
            // When tutorial gets fixed enable this instead of the one below

            //if (datos.Child("Date").Child("scene").Value.ToString() == "Lobby")
            //{
            //    //Debug.Log("Loading Lobby");
            //    PhotonNetwork.JoinLobby();
            //    yield return null;
            //}
            //else if (datos.Child("Date").Child("scene").Value.ToString() == "Tutorial")
            //{
            //    Debug.Log("Loading Tutorial");
            //    GetComponent<DeckManager>().StartGame(false);
            //    GetComponent<PhotonInit>().CreateRoomTutorial("Tutorial");
            //    yield return null;
            //}

            Debug.Log("Loading Lobby");
            PhotonNetwork.JoinLobby();
        }
        await Task.Yield();
    }
    public void reloadDate()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("users").Child(userp.UserId)
      .GetValueAsync().ContinueWith(task =>
      {
          if (task.IsFaulted)
          {
              reloadDate();
          }
          if (task.IsCompleted)
          {
              DatosUser = task.Result;
          }

          if (task.Status == TaskStatus.RanToCompletion)
          {
              onDataReloadedComplete?.Invoke();
              return;
          }
      });
    }
    public Friend LoadFriendData(string friend)
    {
        Friend f = new Friend();
        FirebaseDatabase.DefaultInstance
     .GetReference("users").Child(friend)
     .GetValueAsync().ContinueWith(task =>
     {
         if (task.IsFaulted)
         {
             LoadFriendData(friend);
         }
         else if (task.IsCompleted)
         {
             f.Id = friend;
             f.level = int.Parse(task.Result.Child("Date").Child("level").Value.ToString());
             f.nickname = task.Result.Child("Date").Child("username").Value.ToString();
             f.destreza = int.Parse(task.Result.Child("Date").Child("destreza").Value.ToString());
             f.ultcon = task.Result.Child("Date").Child("ultcon").Value.ToString();
             f.team = task.Result.Child("Date").Child("team").Value.ToString();

         }
     });
        return f;
    }
    public List<string> LoadFriendList()
    {
        List<string> Amigos = new List<string>();
        for (int i = 0; i < DatosUser.Child("Date").Child("friends").ChildrenCount; i++)
        {
            Amigos.Add(DatosUser.Child("Date").Child("friends").Child(i.ToString()).Value.ToString());
        }
        return Amigos;
    }
    public async Task writeNewUser(string userId, string name, string email, int destreza, int coins, int diamond, int tokens, int Etokens, string ultcon, string scene, string deckid, string deck1id, string deck2id, string deck3id, List<string> Cartas, List<string> friends, string cmail)
    {
        int level = 1;
        bool Mstatus = false;
        int exp = 0;
        int dice = 10;
        int gift = 0;

        string giftUnlocked = "None";
        Debug.Log("nuevo user realtime database");
        User user = new User(name, email, destreza, coins, diamond, tokens, Etokens, Mstatus, ultcon, scene, deckid, level, exp, dice, gift, giftUnlocked, friends, cmail);
        Deck deck1 = new Deck(deck1id, Cartas);
        Deck deck2 = new Deck(deck2id, Cartas);
        Deck deck3 = new Deck(deck3id, Cartas);

        var usersRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId);
        await usersRef .Child("Date").SetRawJsonValueAsync(JsonUtility.ToJson(user));
        await usersRef .Child("deck1").SetRawJsonValueAsync(JsonUtility.ToJson(deck1));
        await usersRef.Child("deck2").SetRawJsonValueAsync(JsonUtility.ToJson(deck2));
        await usersRef.Child("deck3").SetRawJsonValueAsync(JsonUtility.ToJson(deck3));
    }
    public void writeNewResultBattleInfo(string GuidBattle, string Date, int Win, string DestrezaWin, string IDWin, string TeamWin, string DestrezaLose, string IDLose, string TeamLose)
    {
        int DestrezaWinGanada = (int)(int.Parse(DestrezaWin) * 0.05f);
        int DestrezaLosePerdida = (int)(int.Parse(DestrezaLose) * 0.05f);

        int DestrezaInfo = DestrezaWinGanada + DestrezaLosePerdida;
        int DestrezaWinTotal = int.Parse(DestrezaWin) + DestrezaInfo;

        //Debug.Log("Nueva battle Master realtime database");
        BattleResult.BattleResultInfo ResultMaster = new BattleResult.BattleResultInfo(Date, DestrezaInfo, Win);

        string resultinfobattle = JsonUtility.ToJson(ResultMaster);
        FirebaseDatabase.DefaultInstance.GetReference("battles").Child(GuidBattle).SetRawJsonValueAsync(resultinfobattle);

        BattleResult.BattleResultDateUser ResultWinDate = new BattleResult.BattleResultDateUser(DestrezaWinGanada.ToString(), IDWin, TeamWin);

        BattleResult.BattleResultDateUser ResultLoseDate = new BattleResult.BattleResultDateUser("-" + DestrezaLosePerdida.ToString(), IDLose, TeamLose);

        if (BattleManager.DiceBet > 0)
        {
            var diceRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDWin).Child("Date").Child("dice");
            diceRef.GetValueAsync().ContinueWith(x =>
            {
                int dice = int.Parse(x.Result.Value.ToString());
                diceRef.SetValueAsync(dice + BattleManager.DiceBet * 2);
            });
        }

        if (Win == 0)
        {
            string resultwininfo = JsonUtility.ToJson(ResultWinDate);
            FirebaseDatabase.DefaultInstance.GetReference("battles").Child(GuidBattle).Child("0").SetRawJsonValueAsync(resultwininfo);
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDWin).Child("Date").Child("destreza").SetValueAsync(DestrezaWinTotal);

            FirebaseDatabase.DefaultInstance
              .GetReference("users").Child(IDWin)
              .GetValueAsync().ContinueWith(task1 =>
              {
                  if (task1.IsFaulted)
                  {
                  }
                  if (task1.IsCompleted)
                  {
                      int Exp = int.Parse(task1.Result.Child("Date").Child("exp").Value.ToString());
                      if (Exp <= 0)
                      {
                          Exp = 2;
                      }
                      Debug.Log(Exp);
                      int Destreza = DestrezaInfo / 2;
                      Exp += Destreza;
                      if (Exp <= 0)
                      {
                          Exp = 2;
                      }
                      Debug.Log(Exp);
                      FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDWin).Child("Date").Child("exp").SetValueAsync(Exp);
                  }
              });

            string resultloseinfo = JsonUtility.ToJson(ResultLoseDate);
            FirebaseDatabase.DefaultInstance.GetReference("battles").Child(GuidBattle).Child("1").SetRawJsonValueAsync(resultloseinfo);

            FirebaseDatabase.DefaultInstance
              .GetReference("users").Child(IDLose)
              .GetValueAsync().ContinueWith(task2 =>
              {
                  if (task2.IsFaulted)
                  {
                  }
                  if (task2.IsCompleted)
                  {
                      int Exp = int.Parse(task2.Result.Child("Date").Child("exp").Value.ToString());
                      if (Exp <= 0)
                      {
                          Exp = 2;
                      }
                      Debug.Log(Exp);
                      int Destreza = DestrezaInfo / 4;
                      Exp += Destreza;
                      if (Exp <= 0)
                      {
                          Exp = 2;
                      }
                      Debug.Log(Exp);
                      FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDLose).Child("Date").Child("exp").SetValueAsync(Exp);
                  }
              });

        }
        else if (Win == 1)
        {
            string resultwininfo = JsonUtility.ToJson(ResultWinDate);
            FirebaseDatabase.DefaultInstance.GetReference("battles").Child(GuidBattle).Child("1").SetRawJsonValueAsync(resultwininfo);
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDWin).Child("Date").Child("destreza").SetValueAsync(DestrezaWinTotal);

            FirebaseDatabase.DefaultInstance
                .GetReference("users").Child(IDWin)
                  .GetValueAsync().ContinueWith(task3 =>
                  {
                      if (task3.IsFaulted)
                          if (task3.IsFaulted)
                          {
                          }
                      if (task3.IsCompleted)
                      {
                          int Exp = int.Parse(task3.Result.Child("Date").Child("exp").Value.ToString());
                          if (Exp <= 0)
                          {
                              Exp = 2;
                          }
                          //Debug.Log(Exp);
                          int Destreza = DestrezaInfo / 2;
                          Exp += Destreza;
                          if (Exp <= 0)
                          {
                              Exp = 2;
                          }
                          //Debug.Log(Exp);
                          FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDWin).Child("Date").Child("exp").SetValueAsync(Exp);
                      }
                  });

            string resultloseinfo = JsonUtility.ToJson(ResultLoseDate);
            FirebaseDatabase.DefaultInstance.GetReference("battles").Child(GuidBattle).Child("0").SetRawJsonValueAsync(resultloseinfo);

            FirebaseDatabase.DefaultInstance
                .GetReference("users").Child(IDLose)
                .GetValueAsync().ContinueWith(task4 =>
                {
                    if (task4.IsFaulted)
                    {
                    }
                    if (task4.IsCompleted)
                    {
                        int Exp = int.Parse(task4.Result.Child("Date").Child("exp").Value.ToString());
                        if (Exp <= 0)
                        {
                            Exp = 2;
                        }
                        //Debug.Log(Exp);
                        int Destreza = DestrezaInfo / 4;
                        Exp += Destreza;
                        if (Exp <= 0)
                        {
                            Exp = 2;
                        }
                        //Debug.Log(Exp);
                        FirebaseDatabase.DefaultInstance.GetReference("users").Child(IDLose).Child("Date").Child("exp").SetValueAsync(Exp);
                    }
                });
        }
    }
}
