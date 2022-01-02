using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Database;
using UnityEngine.SceneManagement;
using TMPro;

public class CardLoaded : MonoBehaviour
{
    public static bool LanzarCarta;
    public GameObject[] Active;

    public GameObject[] CartasPlayer;
    public GameObject[] CartasPlayerNext;

    GameObject Launcher;

    [HideInInspector]
    public List<Card> Cartas;
    [HideInInspector]
    public string team;

    private List<CardPlayer> drawingCardsAnimated = new List<CardPlayer>();

    PhotonView MyView;
    void Awake()
    {
        Launcher = GameObject.FindWithTag("Launcher");
        MyView = GetComponent<PhotonView>();
        StartGame(Launcher.GetComponent<DeckManager>().CartasDeckSelected);

        DragNDrop.onCardUsed += DrawCardAnimation;
    }

    void StartGame(List<Card> Carta)
    {
        Cartas.Clear();
        Cartas.AddRange(Carta);
        AddCardByDeckToGame(Carta);   
    }

    void AddCardByDeckToGame(List<Card> Carta)
    {
        for (int i = 0; i < CartasPlayer.Length; i++)// cartas en la barra inferior
        {
            CartasPlayer[i].GetComponent<CardPlayer>().Carta = null;
        }
        for (int i = 0; i < CartasPlayerNext.Length; i++) //cartas en la barra lateral
        {
            //CartasPlayerNext[i].GetComponent<CardNext>().Carta = GetUnicoAleatorio();
            LoadNextCard(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < CartasPlayerNext.Length; i++)
        {
            if (CartasPlayerNext[i].GetComponent<CardNext>().Carta == null)
            {
                LoadNextCard(i);
            }
        }
        for (int i = 0; i < CartasPlayer.Length; i++)
        {
            if (CartasPlayer[i].GetComponent<CardPlayer>().Carta == null)
            {
                if (CartasPlayer[i].activeInHierarchy)
                {
                    CartasPlayer[i].GetComponent<CardPlayer>().Carta = GetCartaAleatorio();
                    if (CartasPlayer[i].activeInHierarchy)
                    {
                        CartasPlayer[i].GetComponent<CardPlayer>().LoadData();
                    }
                    else
                    {
                        CartasPlayer[i].SetActive(true);
                        CartasPlayer[i].GetComponent<CardPlayer>().LoadData();
                    }
                }
            }
        }
        for (int i = 0; i < CartasPlayer.Length; i++)
        {
            if (CartasPlayer[i].activeInHierarchy && CartasPlayer[i].GetComponent<CardPlayer>().team != team)
            {
                CartasPlayer[i].GetComponent<CardPlayer>().team = team;
            }
        }

        foreach (var drawingCard in drawingCardsAnimated)
        {
            if (drawingCard.DrawAnimation())
            {
                drawingCardsAnimated.Remove(drawingCard);
                return;
            }
        }
    }

    private void DrawCardAnimation(CardPlayer usedCard)
    {
        usedCard.StartDrawing(CartasPlayerNext[0].transform.position, CartasPlayerNext[0].transform.localScale);
        drawingCardsAnimated.Add(usedCard);
    }

    void LoadNextCard(int i)
    {
        CartasPlayerNext[i].GetComponent<CardNext>().Carta = GetUnicoAleatorio();
        if (CartasPlayerNext[i].GetComponent<CardNext>().Carta != null && CartasPlayerNext[i].activeInHierarchy)
        {
            CartasPlayerNext[i].GetComponent<CardNext>().LoadImage();
        }
        else
        {
            CartasPlayerNext[i].GetComponent<Image>().enabled = false;
        }
    }

    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().name =="Tutorial")
        {
            for (int i = 0; i < Active.Length; i++)
            {
                Active[i].SetActive(true);
            }
            for (int i = 0; i < CartasPlayerNext.Length; i++) //cartas en la barra lateral
            {
                if (CartasPlayerNext[i].activeInHierarchy)
                {
                    CartasPlayerNext[i].GetComponent<CardNext>().LoadImage();
                } 
            }
        }
        else
        {
            if (MyView.IsMine)
            {
                for (int i = 0; i < Active.Length; i++)
                {
                    Active[i].SetActive(true);
                }
            }
            for (int i = 0; i < CartasPlayerNext.Length; i++) //cartas en la barra lateral
            {
                if (CartasPlayerNext[i].activeInHierarchy)
                {
                    CartasPlayerNext[i].GetComponent<CardNext>().LoadImage();
                }
            }
        }  
    }
    public Card GetUnicoAleatorio()
    {
        try
        {
            int rand = UnityEngine.Random.Range(0, Cartas.Count);
            Card value = Cartas[rand];
            Cartas.RemoveAt(rand);
            return value;
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }
    public Card GetCartaAleatorio()
    {
        Card New = CartasPlayerNext[0].GetComponent<CardNext>().Carta;
        CartasPlayerNext[0].GetComponent<CardNext>().Carta = CartasPlayerNext[1].GetComponent<CardNext>().Carta;
        CartasPlayerNext[0].GetComponent<CardNext>().LoadImage();       
        CartasPlayerNext[1].GetComponent<CardNext>().Carta = null;
        return New;
    }
    //------------------------------PhotonFunc----------------------------------
    public void SendDataBattleFunct()
    {
        string GuidBattle = Guid.NewGuid().ToString();
        string Date = DateTime.UtcNow.ToString("MM'/'dd'/'yyyy' 'HH':'mm':'ss");
        string IdUser = Launcher.GetComponent<UserDbInit>().DatosUser.Key;
        string Destreza = Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("destreza").Value.ToString();
        MyView.RPC("SendDataBattle", RpcTarget.Others, GuidBattle, Date, IdUser, Destreza);
    }
    void SendDataBattleFunctResponse(string GuidBattle,string Date)
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        MyView = GetComponent<PhotonView>();

        string IdUser = Launcher.GetComponent<UserDbInit>().DatosUser.Key;
        string Destreza = Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("destreza").Value.ToString();
        
        MyView.RPC("SendDataBattleReponse", RpcTarget.Others, GuidBattle, Date, IdUser, Destreza);
    }
    [PunRPC]
    void SendDataBattle(string GuidBattle, string Date, string IdUser, string Destreza)
    {
        BattleManager.instance.GuidBattle = GuidBattle;
        BattleManager.instance.Date = Date;
        BattleManager.instance.IdUserLose = IdUser;
        BattleManager.instance.DestrezaLose = Destreza;
        CalcDestreza(int.Parse(Destreza));
        SendDataBattleFunctResponse(GuidBattle, Date);
    }
    [PunRPC]
    void SendDataBattleReponse(string GuidBattle, string Date,string IdUser, string Destreza)
    {
        BattleManager.instance.GuidBattle = GuidBattle;
        BattleManager.instance.Date = Date;
        BattleManager.instance.IdUserLose = IdUser;
        BattleManager.instance.DestrezaLose = Destreza;
        CalcDestreza(int.Parse(Destreza));
    }
    void CalcDestreza(int DestrezaOther)
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        int Destreza = int.Parse(Launcher.GetComponent<UserDbInit>().DatosUser.Child("Date").Child("destreza").Value.ToString());
        int DestrezaTotal = (int)(DestrezaOther * 0.05f) + (int)(Destreza * 0.05f);
        Destreza -= DestrezaTotal;

        string Id = Launcher.GetComponent<UserDbInit>().DatosUser.Key;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(Id).Child("Date").Child("destreza").SetValueAsync(Destreza);
    }
    //----------------------------------------------------------------
}