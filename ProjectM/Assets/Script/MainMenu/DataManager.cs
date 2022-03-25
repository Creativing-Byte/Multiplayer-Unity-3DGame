using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    UserDbInit Launcher;
    FirebaseAuth auth;

    [Header("User data Hud")]
    public Text[] Divisas; // 0 trofeos, 1 monedas, 2 diamantes
    public TextMeshProUGUI TextLoading;
    public TextMeshProUGUI[] CasinoDivisas; // 0 tokens, 1 Etokens, 2 Etokens(Userinfo)
    public TextMeshProUGUI InfoUsername;
    public TextMeshProUGUI InfoTrofeos;
    public TextMeshProUGUI Infolevel;
    public LobbyControl Lobbycontrol;
    public Text username;
    public Image progress;
    public Button UserInfo;
    public GameObject UserInfoPanel;

    [Header("User Exp System")]
    public Image BarExp;
    public Text Exp, Level;
    public int MaxLevels = 60, ExpInitLevel = 300, ExpLastLevel = 3000000;
    int LevelCurrent, ExpCurrent, ExpMax;
    public int levelactualtext;
    public List<int> expsum;

    [Header("User Gift System")]
    public Text TimeGift;
    public GameObject OpenNow, GemUnlock;

    [Header("Maps")]
    public List<Sprite> MapSprites;
    public Image LobbyCurrentMap;
    public TMP_Text CurrentArenaText;

    int gift;
    int currentlyChosenLevel = 1;
    string giftTimeUnlocked;

    public Text VersionInfo;

    private DatabaseReference UserDataRef;

    void Start()
    {
        UserInfo.onClick.AddListener(OnOptionsChange);
        Launcher = GameObject.FindGameObjectWithTag("Launcher").GetComponent<UserDbInit>();

        UserDataRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(Launcher.DatosUser.Key).Child("Date");

        Divisas[0].text = Launcher.DatosUser.Child("Date").Child("destreza").Value.ToString();
        InfoTrofeos.text = Launcher.DatosUser.Child("Date").Child("destreza").Value.ToString();
        Divisas[1].text = Launcher.DatosUser.Child("Date").Child("coins").Value.ToString();
        Divisas[2].text = Launcher.DatosUser.Child("Date").Child("diamond").Value.ToString();

        CasinoDivisas[0].text = Launcher.DatosUser.Child("Date").Child("tokens").Value.ToString();
        CasinoDivisas[1].text = Launcher.DatosUser.Child("Date").Child("Etokens").Value.ToString();
        CasinoDivisas[2].text = Launcher.DatosUser.Child("Date").Child("Etokens").Value.ToString();

        username.text = Launcher.DatosUser.Child("Date").Child("username").Value.ToString();
        username.gameObject.transform.GetChild(0).GetComponent<Text>().text = username.text;


        LevelCurrent = int.Parse(Launcher.DatosUser.Child("Date").Child("level").Value.ToString());
        Level.text = LevelCurrent.ToString();

        ExpCurrent = int.Parse(Launcher.DatosUser.Child("Date").Child("exp").Value.ToString());
        ExpMax = ExpToLevel(int.Parse(Level.text));

        gift = int.Parse(Launcher.DatosUser.Child("Date").Child("gift").Value.ToString());

        giftTimeUnlocked = Launcher.DatosUser.Child("Date").Child("giftUnlocked").Value.ToString();

        UserDataRef.ValueChanged += DateValueChanged;

        InfoUsername.text = Launcher.DatosUser.Child("Date").Child("username").Value.ToString();
        Infolevel.text = LevelCurrent.ToString();

        UpdateDate();
    }

    void Update()
    {
        //expsum.Clear();
        //for (int i = 0; i < MaxLevels; i++)
        //{
        //    expsum.Add(ExpToLevel(levelactualtext));
        //    if (levelactualtext >= MaxLevels)
        //    {
        //        levelactualtext = MaxLevels;
        //    }
        //}

        print(Launcher.DatosUser.Key);

        VersionInfo.text = "Version: " + Application.version;
        float Range = 1.0f / ExpMax * ExpCurrent;
        BarExp.fillAmount = Range;
        progress.fillAmount = float.Parse(Launcher.DatosUser.Child("Date").Child("destreza").Value.ToString())/20;

        Exp.text = ExpCurrent + "/" + ExpMax;
        Exp.gameObject.transform.GetChild(0).GetComponent<Text>().text = Exp.text;
        Level.gameObject.transform.GetChild(0).GetComponent<Text>().text = Level.text;
        username.gameObject.transform.GetChild(0).GetComponent<Text>().text = username.text;

        //if (gift == 1)
        //{
        //    DateTime now = DateTime.UtcNow;
        //    CultureInfo provider = CultureInfo.CurrentCulture;
        //    DateTime old = DateTime.ParseExact(giftTimeUnlocked, "MM'/'dd'/'yyyy' 'HH':'mm':'ss", provider);
        //    TimeSpan diff = now - old;
        //    if (diff.TotalDays >= 0.5f)
        //    {
        //        TimeGift.text = "Open Now";
        //        OpenNow.GetComponent<Text>().text = "Open Now";
        //        OpenNow.transform.GetChild(0).GetComponent<Text>().text = "Open Now";
        //        GemUnlock.SetActive(false);
        //    }
        //    else if (diff.TotalDays < 0.5f && diff.Hours < 12)
        //    {
        //        TimeGift.text = diff.Hours + ":" + diff.Minutes + ":" + diff.Seconds;
        //        OpenNow.GetComponent<Text>().text = "Unlocking Gift";
        //        OpenNow.transform.GetChild(0).GetComponent<Text>().text = "Unlocking Gift";
        //        GemUnlock.SetActive(true);
        //    }
        //}
        //else
        //{
        //    TimeGift.text = "--:--:--";
        //    OpenNow.GetComponent<Text>().text = "Start Unlocking";
        //    OpenNow.transform.GetChild(0).GetComponent<Text>().text = "Start Unlocking";
        //    GemUnlock.SetActive(false);
        //}
    }
    void UpdateDate()
    {
        if (ExpCurrent >= ExpToLevel(LevelCurrent))
        {
            ExpCurrent = 0;
            UserDataRef.Child("level").SetValueAsync(1 + int.Parse(Level.text));
            UserDataRef.Child("exp").SetValueAsync(0);
        }
    }
    public void OnOptionsChange()
    {
        if (UserInfoPanel != null)
        {
            UserInfoPanel.SetActive(!UserInfoPanel.activeSelf);
        }
    }
    void DateValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        try
        {
            ExpCurrent = int.Parse(args.Snapshot.Child("exp").Value.ToString());
            Level.text = args.Snapshot.Child("level").Value.ToString();
            ExpMax = ExpToLevel(int.Parse(Level.text));
            if (int.Parse(args.Snapshot.Child("destreza").Value.ToString()) < 0)
            {
                UserDataRef.Child("destreza").SetValueAsync(0);
            }
            if (int.Parse(args.Snapshot.Child("coins").Value.ToString()) < 0)
            {
                UserDataRef.Child("coins").SetValueAsync(0);
            }
            if (int.Parse(args.Snapshot.Child("diamond").Value.ToString()) < 0)
            {
                UserDataRef.Child("diamond").SetValueAsync(0);
            }
            if (int.Parse(args.Snapshot.Child("tokens").Value.ToString()) < 0)
            {
                UserDataRef.Child("tokens").SetValueAsync(0);
            }
            if (int.Parse(args.Snapshot.Child("Etokens").Value.ToString()) < 0)
            {
                UserDataRef.Child("Etokens").SetValueAsync(0);
            }
            if (int.Parse(args.Snapshot.Child("dice").Value.ToString()) < 0)
            {
                UserDataRef.Child("dice").SetValueAsync(0);
            }
            Divisas[0].text = args.Snapshot.Child("destreza").Value.ToString();
            InfoTrofeos.text = args.Snapshot.Child("destreza").Value.ToString();
            Divisas[1].text = args.Snapshot.Child("coins").Value.ToString();
            Divisas[2].text = args.Snapshot.Child("diamond").Value.ToString();

            CasinoDivisas[0].text = args.Snapshot.Child("tokens").Value.ToString();
            CasinoDivisas[1].text = args.Snapshot.Child("Etokens").Value.ToString();
            CasinoDivisas[2].text = args.Snapshot.Child("Etokens").Value.ToString();


            username.text = args.Snapshot.Child("username").Value.ToString();


            gift = int.Parse(args.Snapshot.Child("gift").Value.ToString());
            giftTimeUnlocked = args.Snapshot.Child("giftUnlocked").Value.ToString();

            InfoUsername.text = args.Snapshot.Child("username").Value.ToString();
            Infolevel.text = args.Snapshot.Child("level").Value.ToString();

            UpdateDate();
        }
        catch (MissingReferenceException)
        {

        }
        catch (NullReferenceException)
        {

        }
    }
    public async void MatchMakind(int diceBet)
    {
        BattleManager.DiceBet = diceBet;

        if (diceBet > 0)
        {
            var diceRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(Launcher.DatosUser.Key).Child("Date").Child("dice");
            if (int.Parse((await diceRef.GetValueAsync()).Value.ToString()) < diceBet)
            {
                PopupMessage.instance.Show("Not enough tokens");
                return;
            }
        }

        Lobbycontrol.ActivarPanelLoading();

        PhotonInit.PhotonInitInstance.switchToRoom("World" + currentlyChosenLevel, true);
        ChangedDeck();

        TextLoading.text = "Searching Oponents";
    }

    public void SetLevel(int level)
    {
        currentlyChosenLevel = level;
        LobbyCurrentMap.sprite = MapSprites[level - 1];
        CurrentArenaText.text = $"Arena {level}";
    }

    public void ReloadScene()
    {
        Lobbycontrol.ResetTimer();
        PhotonInit.PhotonInitInstance.switchToRoom("Lobby", false);
        TextLoading.text = "Canceling battle search";
        ChangedDeck();
    }
    public void LogOut()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
    }
    void ChangedDeck()
    {
        Launcher.GetComponent<DeckManager>().StartGame(true);
    }
    int ExpToLevel(int Level)
    {
        float B = Mathf.Log(1 * ExpLastLevel / ExpInitLevel) / (MaxLevels - 1);
        float A = 1 * ExpInitLevel / Mathf.Exp(B);

        int x = (int)(A * Mathf.Exp(B * Level));
        int y = 10 ^ (int)(Mathf.Log(x) / Mathf.Log(10));
        if (((x / y) * y) < ExpInitLevel)
        {
            return ExpInitLevel;
        }
        else if (((x / y) * y) > ExpLastLevel)
        {
            return ExpLastLevel;
        }
        return (x / y) * y;
    }


    public void UnlockGift()
    {
        if (gift == 0)
        {
            UserDataRef.Child("giftUnlocked").SetValueAsync(DateTime.UtcNow.ToString("MM'/'dd'/'yyyy' 'HH':'mm':'ss"));
            UserDataRef.Child("gift").SetValueAsync(1);
        }
        else if (gift == 1 && TimeGift.text == "Open Now")
        {
            UserDataRef.Child("gift").SetValueAsync(0);
            UserDataRef.Child("giftUnlocked").SetValueAsync("None");
            GiftReward();
        }
        else if (gift == 1 && TimeGift.text != "Open Now" && int.Parse(Divisas[2].text) >= 15)
        {
            int gem = int.Parse(Divisas[2].text);
            gem -= 15;
            UserDataRef.Child("diamond").SetValueAsync(gem);
            UserDataRef.Child("gift").SetValueAsync(0);
            UserDataRef.Child("giftUnlocked").SetValueAsync("None");
            GiftReward();
        }
    }
    void GiftReward()
    {
        bool oro = UnityEngine.Random.Range(0, 10) == 1;
        bool diamond = UnityEngine.Random.Range(0, 30) == 1;
        bool dice = UnityEngine.Random.Range(0, 50) == 1;

        if (oro)
        {
            int OroReward = int.Parse(Divisas[1].text);
            OroReward += UnityEngine.Random.Range(1, 10001);
            Debug.Log("Oro " + OroReward);
            UserDataRef.Child("coins").SetValueAsync(OroReward);
        }

        if (diamond)
        {
            int DiamondReward = int.Parse(Divisas[2].text);
            DiamondReward += UnityEngine.Random.Range(1, 101);
            Debug.Log("Diamond " + DiamondReward);
            UserDataRef.Child("diamond").SetValueAsync(DiamondReward);
        }

        if (dice)
        {
            int DiceReward = int.Parse(Launcher.DatosUser.Child("Date").Child("dice").Value.ToString());
            DiceReward += UnityEngine.Random.Range(1, 11);
            Debug.Log("Dice " + DiceReward);
            UserDataRef.Child("dice").SetValueAsync(DiceReward);
        }

        if (!oro && !diamond && !dice)
        {
            int ExpReward = ExpCurrent;
            ExpReward += UnityEngine.Random.Range(1, 51);
            Debug.Log("Exp " + ExpReward);
            UserDataRef.Child("exp").SetValueAsync(ExpReward);
        }

    }

    private void OnDestroy() 
        => UserDataRef.ValueChanged -= DateValueChanged;

#if UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Firebase.FirebaseApp.DefaultInstance.Dispose();
    }
#elif !UNITY_EDITOR
    private void OnApplicationQuit()
    {
        Firebase.FirebaseApp.DefaultInstance.Dispose();
    }
#endif
}
