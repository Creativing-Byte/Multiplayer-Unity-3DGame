using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDbInit : MonoBehaviour
{
    DataSnapshot Cards;
    void Start() => UpdateCards();

    public async void UpdateCards()
    {
        var @ref = FirebaseDatabase.DefaultInstance.GetReference("Cards");
        @ref.KeepSynced(true);
        Cards = await @ref.GetValueAsync();
    }

    public List<Card> LoadAllCard() => Cards.Children.Select(x => LoadCard(x.Key)).ToList();

    public Card LoadCard(DataSnapshot data)
    {
        if (data == null) return new Card();
        Card carta = new Card
        {
            id = Convert.ToUInt16(data.Child("id").Value.ToString()),
            name = data.Child("name").Value.ToString(),
            tipe = data.Child("tipe").Value.ToString(),
            spawn = Convert.ToUInt16(data.Child("spawn").Value.ToString()),
            time = Convert.ToUInt16(data.Child("time").Value.ToString()),
            level = Convert.ToUInt16(data.Child("level").Value.ToString()),
            Prefabs = data.Child("Prefabs").Value.ToString(),
            vida = Convert.ToUInt16(data.Child("vida").Value.ToString()),
            ataque = Convert.ToUInt16(data.Child("ataque").Value.ToString()),
            velocidad = float.Parse(data.Child("velocidad").Value.ToString()),
            vataque = float.Parse(data.Child("vataque").Value.ToString()),
            range = float.Parse(data.Child("range").Value.ToString()),
            objetivos = data.Child("objetivos").Value.ToString(),
            decripcion = data.Child("decripcion").Value.ToString(),
            photo = data.Child("photo").Value.ToString(),
            throwable = bool.Parse(data.Child("throwable").Value.ToString())
        };
        return carta;
    }

    public Card LoadCard(string id) => LoadCard(Cards.Child(id));
}
