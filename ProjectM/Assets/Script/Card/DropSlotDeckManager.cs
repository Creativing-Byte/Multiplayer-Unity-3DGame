using Firebase;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlotDeckManager : MonoBehaviour
{
    Card cardold;
    Card cardnew;
    GameObject Launcher;
    GameObject Event;

    DatabaseReference reference;

    GameObject itemdragging;
    void Start()
    {
        Launcher = GameObject.FindGameObjectWithTag("Launcher");
        Event = GameObject.FindGameObjectWithTag("EventSystem");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void itemdraggingdrop(GameObject item)
    {
        itemdragging = item;
        Event.SetActive(false);

        if (GetComponent<CardInfo>().deckmanager == false && itemdragging.GetComponent<CardInfo>().deckmanager == false)
        {
            if (GetComponent<CardInfo>().Carta.id != itemdragging.GetComponent<CardInfo>().Carta.id)
            {
                UpdateCardsToCards();
            }
            else
            {
                Event.SetActive(true);
                cardold = null;
                cardnew = null;
                itemdragging.GetComponent<Image>().enabled = false;
            }
        }
        else if (GetComponent<CardInfo>().deckmanager == true && itemdragging.GetComponent<CardInfo>().deckmanager == false)
        {
            UpdateCardsToDeck(1);
        }
        else if (GetComponent<CardInfo>().deckmanager == false && itemdragging.GetComponent<CardInfo>().deckmanager == true)
        {
            UpdateCardsToDeck(0);
        }
        else
        {
            Event.SetActive(true);
            cardold = null;
            cardnew = null;
            itemdragging.GetComponent<Image>().enabled = false;
        }
    }
    async void UpdateCardsToCards()
    {
        cardold = GetComponent<CardInfo>().Carta;
        cardnew = itemdragging.GetComponent<CardInfo>().Carta;

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        Dictionary<string, object> childUpdates1 = new Dictionary<string, object>();

        //string deck = "deck" + DeckSelected.deckselected.ToString();

        //var cardsRef = reference.Child("users").Child(Launcher.GetComponent<UserDbInit>().DatosUser.Key).Child(deck).Child("Cartas");
        //await cardsRef
        //    .Child(itemdragging.GetComponent<CardInfo>().idpos.ToString())
        //    .SetValueAsync(GetComponent<CardInfo>().Carta.id.ToString());

        //GetComponent<CardInfo>().Carta = cardnew;

        //await cardsRef
        //    .Child(GetComponent<CardInfo>().idpos.ToString())
        //    .SetValueAsync(itemdragging.GetComponent<CardInfo>().Carta.id.ToString());

        //itemdragging.GetComponent<CardInfo>().Carta = cardold;
        //itemdragging.GetComponent<CardInfo>().LoadImage();
        //GetComponent<CardInfo>().LoadImage();
        //cardnew = null;
        //cardold = null;
        //Event.SetActive(true);
        //itemdragging.GetComponent<Image>().enabled = false;
    }

    void UpdateCardsToDeck(int num)
    {
        //AllCardsDb ReloadAllCardsDb = GameObject.FindGameObjectWithTag("DeckSelected").GetComponent<DeckSelected>().AlldbDecks.GetComponent<AllCardsDb>();
        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        Dictionary<string, object> childUpdates1 = new Dictionary<string, object>();

        //string deck = "deck" + DeckSelected.deckselected.ToString();

        //if (num == 0) // del deck manager al deck del usuario, itemdraggin es del deck manager
        //{
        //    cardold = GetComponent<CardInfo>().Carta;
        //    int idpos = GetComponent<CardInfo>().idpos;

        //    cardnew = itemdragging.GetComponent<CardInfo>().Carta;

        //    reference.Child("users").Child(Launcher.GetComponent<UserDbInit>().DatosUser.Key).Child(deck).Child("Cartas")
        //        .Child(GetComponent<CardInfo>().idpos.ToString())
        //        .SetValueAsync(itemdragging.GetComponent<CardInfo>().Carta.id.ToString());

        //    GetComponent<CardInfo>().Carta = cardnew;
        //    GetComponent<CardInfo>().idpos = idpos;
        //    GetComponent<CardInfo>().deckmanager = false;
        //    GetComponent<CardInfo>().LoadImage();

        //    cardnew = null;
        //    cardold = null;
        //    Event.SetActive(true);
        //    itemdragging.GetComponent<Image>().enabled = false;
        //    //ReloadAllCardsDb.UnavalibleCards(DeckSelected.deckselected);
        //}
        //if (num == 1) // del usuario al deck manager,  itemdraggin es del deck del usuario
        //{
        //    cardold = GetComponent<CardInfo>().Carta;
        //    int idpos = GetComponent<CardInfo>().idpos;

        //    cardnew = itemdragging.GetComponent<CardInfo>().Carta;

        //    reference.Child("users").Child(Launcher.GetComponent<UserDbInit>().DatosUser.Key).Child(deck).Child("Cartas")
        //        .Child(itemdragging.GetComponent<CardInfo>().idpos.ToString())
        //        .SetValueAsync(GetComponent<CardInfo>().Carta.id.ToString());
        //    itemdragging.GetComponent<CardInfo>().Carta = cardold;

        //    itemdragging.GetComponent<CardInfo>().LoadImage();

        //    cardnew = null;
        //    cardold = null;
        //    Event.SetActive(true);
        //    itemdragging.GetComponent<Image>().enabled = false;
        //    //ReloadAllCardsDb.UnavalibleCards(DeckSelected.deckselected);
        //}
        //ReloadDate();
    }

    void ReloadDate()
    {
        Launcher.GetComponent<UserDbInit>().reloadDate();
    }
}
