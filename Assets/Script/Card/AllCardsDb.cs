using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCardsDb : MonoBehaviour
{
    GameObject Launcher;

    public GameObject dragparent;

    public GameObject CartaInfoPrefabs;

    public GameObject[] Decks;

    List<Card> deck = new List<Card>();

    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");

        deck = Launcher.GetComponent<CardDbInit>().LoadAllCard();

        loadallcards();
    }
    void loadallcards()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject CartaPrefabs1 = Instantiate(CartaInfoPrefabs, new Vector3(0, 0, 0), Quaternion.identity);
            CartaPrefabs1.gameObject.transform.SetParent(gameObject.transform);
            CartaPrefabs1.gameObject.GetComponent<DragNDropDeckManager>().dragParent = dragparent.transform;
            CartaPrefabs1.gameObject.GetComponent<DragNDropDeckManager>().posFix = new Vector3(235, 400, 0);
            CartaPrefabs1.GetComponent<CardInfo>().Carta = deck[i];
            CartaPrefabs1.GetComponent<CardInfo>().deckmanager = true;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).localScale != new Vector3(1, 1, 0))
            {
                transform.GetChild(i).localScale = new Vector3(1, 1, 0);
            }
        }
        GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x, -406, GetComponent<RectTransform>().localPosition.z);
    }
    public void UnavalibleCards(int num)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        if (num == 1)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < Decks[0].transform.childCount; j++)
                {
                    if (transform.GetChild(i).gameObject.GetComponent<CardInfo>().Carta.id == Decks[0].transform.GetChild(j).gameObject.GetComponent<CardInfo>().Carta.id)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            for (int k = 0; k < transform.childCount; k++)
            {
                if (transform.GetChild(k).gameObject.activeInHierarchy == true)
                {
                    transform.GetChild(k).gameObject.GetComponent<CardInfo>().LoadImage();
                }
            }
        }
        if (num == 2)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < Decks[1].transform.childCount; j++)
                {
                    if (transform.GetChild(i).gameObject.GetComponent<CardInfo>().Carta.id == Decks[1].transform.GetChild(j).gameObject.GetComponent<CardInfo>().Carta.id)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            for (int k = 0; k < transform.childCount; k++)
            {
                if (transform.GetChild(k).gameObject.activeInHierarchy == true)
                {
                    transform.GetChild(k).gameObject.GetComponent<CardInfo>().LoadImage();
                }
            }
        }
        if (num == 3)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < Decks[2].transform.childCount; j++)
                {
                    if (transform.GetChild(i).gameObject.GetComponent<CardInfo>().Carta.id == Decks[2].transform.GetChild(j).gameObject.GetComponent<CardInfo>().Carta.id)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            for (int k = 0; k < transform.childCount; k++)
            {
                if (transform.GetChild(k).gameObject.activeInHierarchy == true)
                {
                    transform.GetChild(k).gameObject.GetComponent<CardInfo>().LoadImage();
                }
            }
        }
    }
}
