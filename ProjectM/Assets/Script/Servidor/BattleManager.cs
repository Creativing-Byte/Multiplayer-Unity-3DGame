using UnityEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Linq;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static int DiceBet;
    public GameObject PanelWin, PanelEmpate, PanelLose;
    public GameObject endGamePanel;
    public string opponentName;
    GameObject Launcher;
    UserDbInit userDb;
    public bool isDraw = false;

    public string GuidBattle, Date, IdUserLose, DestrezaLose;

    public static BattleManager instance;

    public int bluePoints = 0;
    public int redPoints = 0;

    private object userId;

    void Start()
    {
        instance = this;
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        userDb = Launcher.GetComponent<UserDbInit>();

        Init();
    }

    async void Init()
    {
        userId = PhotonNetwork.PlayerList.Where(x => (string)x.CustomProperties["UserId"] != userDb.DatosUser.Key).FirstOrDefault().CustomProperties["UserId"];
        opponentName = (string)(await FirebaseDatabase.DefaultInstance.GetReference($"users/{userId}/Date/username").GetValueAsync()).Value;
        GameObject.FindGameObjectWithTag("EnemyName").GetComponent<TextMeshProUGUI>().text = opponentName;
    }

    public async void EndGame()
    {
        Time.timeScale = 0;

        string winner = null;
        string loser = null;
        int winnerID = -1;

        string myTeam = PhotonInit.MyTeam;
        string enemyTeam = myTeam.ToLower() == "red" ? "blue" : "red"; 
        string opponentName = (string)(await FirebaseDatabase.DefaultInstance.GetReference($"users/{userId}/Date/username").GetValueAsync()).Value;

        if (redPoints > bluePoints)
        {
            winner = "red";
            loser = "blue";
            winnerID = 0;
        }
        else if (bluePoints > redPoints)
        {
            winner = "blue";
            loser = "red";
            winnerID = 1;
        }
        else isDraw = true;

        bool iWon = false;
        if (winner != null && myTeam.ToLower() == winner)
            iWon = true;

        if (iWon)
        {
            string DestrezaWin = userDb.DatosUser.Child("Date").Child("destreza").Value.ToString();
            string IdUser = userDb.DatosUser.Key;

            userDb.writeNewResultBattleInfo(GuidBattle, Date, winnerID, DestrezaWin, IdUser, myTeam, DestrezaLose, IdUserLose, loser);
        }
        userDb.reloadDate();

        string username = userDb.DatosUser.Child("Date/username").Value.ToString();

        var egp = OpenEndGamePanel();
        if (egp != null)
        {
            egp.SetData(iWon, myTeam.ToLower(),
                username, opponentName,
                "", "",
                myTeam.ToLower() == "red" ? redPoints : bluePoints,
                enemyTeam.ToLower() == "red" ? redPoints : bluePoints,
                150, 85, 2, 8);
        }
    }

    public void SetWinTutorial(string TeamLoser)
    {
        string winner_ = TeamLoser.ToLower() == "red" ? "Blue" : "Red";
        bool iWinner = winner_.ToLower() == PhotonInit.MyTeam.ToLower();
        if (iWinner)
        {
            if (PanelWin)
            {
                PanelWin.SetActive(true);
            }
        }
        else
        {
            if (PanelLose)
            {
                PanelLose.SetActive(true);
            }
        }
    }

    public void BackToLobby()
    {
        userDb.reloadDate();
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            GameObject Launcher = GameObject.FindGameObjectWithTag("Launcher");
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(Launcher.GetComponent<UserDbInit>().DatosUser.Key).Child("Date").Child("scene").SetValueAsync("Lobby");
        }
        Time.timeScale = 1;
        PhotonInit.PhotonInitInstance.switchToRoom("Lobby", false);
    }

    private EndGamePanel OpenEndGamePanel()
    {
        if (endGamePanel == null) return null;
        endGamePanel.gameObject.SetActive(true);
        return endGamePanel.GetComponent<EndGamePanel>();
    }
}
