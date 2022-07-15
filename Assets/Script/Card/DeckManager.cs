using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class DeckManager : MonoBehaviour
{
    List<string> Cartasiddeck1 = new List<string>();
    List<string> Cartasiddeck2 = new List<string>();
    List<string> Cartasiddeck3 = new List<string>();

    [HideInInspector]
    public List<Card> CartasDeckSelected = new List<Card>();

    public void StartGame(bool options)
    {
        StartCoroutine(loadAllDeck(options));
    }
    IEnumerator loadAllDeck(bool options)
    {
        if (options)
        {
            CartasDeckSelected.Clear();
            GetComponent<UserDbInit>().reloadDate();
            yield return new WaitForSeconds(1);
            for (int i = 0; i < GetComponent<UserDbInit>().DatosUser.Child("deck1").Child("Cartas").ChildrenCount; i++)
            {
                Cartasiddeck1.Insert(i, GetComponent<UserDbInit>().DatosUser.Child("deck1").Child("Cartas").Child(i.ToString()).Value.ToString());
                Cartasiddeck2.Insert(i, GetComponent<UserDbInit>().DatosUser.Child("deck2").Child("Cartas").Child(i.ToString()).Value.ToString());
                Cartasiddeck3.Insert(i, GetComponent<UserDbInit>().DatosUser.Child("deck3").Child("Cartas").Child(i.ToString()).Value.ToString());
            }
            AddCardToDeck();
            yield return null;
        }
        else
        {
            CartasDeckSelected.Clear();
            for (int i = 0; i < GetComponent<UserDbInit>().DatosUser.Child("deck1").Child("Cartas").ChildrenCount; i++)
            {
                Cartasiddeck1.Insert(i, GetComponent<UserDbInit>().DatosUser.Child("deck1").Child("Cartas").Child(i.ToString()).Value.ToString());
                Cartasiddeck2.Insert(i, GetComponent<UserDbInit>().DatosUser.Child("deck2").Child("Cartas").Child(i.ToString()).Value.ToString());
                Cartasiddeck3.Insert(i, GetComponent<UserDbInit>().DatosUser.Child("deck3").Child("Cartas").Child(i.ToString()).Value.ToString());
            }
            AddCardToDeck();
            yield return null;
        }
        yield return null;
    }
    void AddCardToDeck()
    {
        string deckid = GetComponent<UserDbInit>().DatosUser.Child("Date").Child("deckid").Value.ToString();
        if (deckid == GetComponent<UserDbInit>().DatosUser.Child("deck1").Child("id").Value.ToString())
        {
            for (int i = 0; i < GetComponent<UserDbInit>().DatosUser.Child("deck1").Child("Cartas").ChildrenCount; i++)
            {
                CartasDeckSelected.Insert(i, GetComponent<CardDbInit>().LoadCard(Cartasiddeck1[i]));
            }
        }
        else if (deckid == GetComponent<UserDbInit>().DatosUser.Child("deck2").Child("id").Value.ToString())
        {
            for (int i = 0; i < GetComponent<UserDbInit>().DatosUser.Child("deck2").Child("Cartas").ChildrenCount; i++)
            {
                CartasDeckSelected.Insert(i, GetComponent<CardDbInit>().LoadCard(Cartasiddeck2[i]));
            }
        }
        else if (deckid == GetComponent<UserDbInit>().DatosUser.Child("deck3").Child("id").Value.ToString())
        {
            for (int i = 0; i < GetComponent<UserDbInit>().DatosUser.Child("deck3").Child("Cartas").ChildrenCount; i++)
            {
                CartasDeckSelected.Insert(i, GetComponent<CardDbInit>().LoadCard(Cartasiddeck3[i]));
            }
        }
    }
}
