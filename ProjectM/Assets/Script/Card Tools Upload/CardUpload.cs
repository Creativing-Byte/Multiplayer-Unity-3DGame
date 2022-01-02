using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUpload : MonoBehaviour
{
    public InputField[] Datos; // 0 Id, 1 name, 2 tipe, 3 spawn, 4 time, 5 level, 6 idprefab, 7 vida, 8 ataque, 9 velocidad
                               // 10 velocidadataque, 11 rango, 12 objectivos, 13 descripcion, 14 guid foto, 15 throwable
    bool Ground, Air;
    public void Bool(int num)
    {
        if (num == 1)
        {
            Air = !Air;
        }
        if (num == 0)
        {
            Ground = !Ground;
        }
    }
    public void throwable(int num)
    {
        if (num == 1)
        {
            Datos[15].text = "true";
        }
        if (num == 0)
        {
            Datos[15].text = "false";
        }
    }
    public void GenerateId()
    {
        FirebaseDatabase.DefaultInstance
     .GetReference("Cards")
     .GetValueAsync().ContinueWith(task =>
     {
         if (task.IsFaulted)
         {
             GenerateId();
         }
         else if (task.IsCompleted)
         {
             Datos[0].text = task.Result.ChildrenCount.ToString();
         }
     });
    }
    public void GenerateTipe()
    {
        if (Ground && Air)
        {
            Datos[12].text = "Ground/Air";
            Ground = false;
            Air = false;
        }
        else
        {
            if (Ground)
            {
                Datos[12].text = "Ground";
                Ground = false;
                Air = false;
            }
            if (Air)
            {
                Datos[12].text = "Air";
                Ground = false;
                Air = false;
            }
        }
    }
    public void GenerateGUID()
    {
        Datos[14].text = Guid.NewGuid().ToString("N").ToUpper();
    }
    public void Upload()
    {
        writeNewCard(Convert.ToInt16(Datos[0].text), Datos[1].text, Datos[2].text, Convert.ToInt16(Datos[3].text), Convert.ToInt16(Datos[4].text), Convert.ToInt16(Datos[5].text), Datos[6].text, Convert.ToInt16(Datos[7].text), Convert.ToInt16(Datos[8].text), float.Parse(Datos[9].text), float.Parse(Datos[10].text), float.Parse(Datos[11].text), Datos[12].text, Datos[13].text, Datos[14].text, Datos[15].text);
    }
    public void Download()
    {
        FirebaseDatabase.DefaultInstance
     .GetReference("Cards")
     .GetValueAsync().ContinueWith(task =>
     {
         if (task.IsFaulted)
         {
             Download();
         }
         else if (task.IsCompleted)
         {
             GenerateId();
             Datos[1 ].text = task.Result.Child("0").Child("name").Value.ToString();
             Datos[2 ].text = task.Result.Child("0").Child("tipe").Value.ToString();
             Datos[3 ].text = task.Result.Child("0").Child("spawn").Value.ToString();
             Datos[4 ].text = task.Result.Child("0").Child("time").Value.ToString();
             Datos[5 ].text = task.Result.Child("0").Child("level").Value.ToString();
             Datos[6 ].text = task.Result.Child("0").Child("Prefabs").Value.ToString();
             Datos[7 ].text = task.Result.Child("0").Child("vida").Value.ToString();
             Datos[8 ].text = task.Result.Child("0").Child("ataque").Value.ToString();
             Datos[9 ].text = task.Result.Child("0").Child("velocidad").Value.ToString();
             Datos[10].text = task.Result.Child("0").Child("vataque").Value.ToString();
             Datos[11].text = task.Result.Child("0").Child("range").Value.ToString();
             Datos[12].text = task.Result.Child("0").Child("objetivos").Value.ToString();
             Datos[13].text = task.Result.Child("0").Child("decripcion").Value.ToString();
             Datos[14].text = task.Result.Child("0").Child("photo").Value.ToString();
         }
     });
    }
    public void DownloadCard()
    {
        int num = int.Parse(Datos[0].text);
        FirebaseDatabase.DefaultInstance
     .GetReference("Cards")
     .GetValueAsync().ContinueWith(task =>
     {
         if (task.IsFaulted)
         {
             Download();
         }
         else if (task.IsCompleted)
         {
             Datos[1 ].text = task.Result.Child(num.ToString()).Child("name").Value.ToString();
             Datos[2 ].text = task.Result.Child(num.ToString()).Child("tipe").Value.ToString();
             Datos[3 ].text = task.Result.Child(num.ToString()).Child("spawn").Value.ToString();
             Datos[4 ].text = task.Result.Child(num.ToString()).Child("time").Value.ToString();
             Datos[5 ].text = task.Result.Child(num.ToString()).Child("level").Value.ToString();
             Datos[6 ].text = task.Result.Child(num.ToString()).Child("Prefabs").Value.ToString();
             Datos[7 ].text = task.Result.Child(num.ToString()).Child("vida").Value.ToString();
             Datos[8 ].text = task.Result.Child(num.ToString()).Child("ataque").Value.ToString();
             Datos[9 ].text = task.Result.Child(num.ToString()).Child("velocidad").Value.ToString();
             Datos[10].text = task.Result.Child(num.ToString()).Child("vataque").Value.ToString();
             Datos[11].text = task.Result.Child(num.ToString()).Child("range").Value.ToString();
             Datos[12].text = task.Result.Child(num.ToString()).Child("objetivos").Value.ToString();
             Datos[13].text = task.Result.Child(num.ToString()).Child("decripcion").Value.ToString();
             Datos[14].text = task.Result.Child(num.ToString()).Child("photo").Value.ToString();
             Datos[15].text = task.Result.Child(num.ToString()).Child("throwable").Value.ToString();
         }
     });
    }
    void writeNewCard(int id, string name, string tipe, int spawn, int time, int level, string idprefab, int vida, int ataque, float velocidad, float velocidada, float rango, string objectivos, string descripcion, string guidfoto, string throwable)
    {
        Debug.Log("nueva Card realtime database");
        Card card = new Card
        {
            id = id,
            name = name,
            tipe = tipe,
            spawn = spawn,
            time = time,
            level = level,
            Prefabs = idprefab,
            vida = vida,
            ataque = ataque,
            velocidad = velocidad,
            vataque = velocidada,
            range = rango,
            objetivos = objectivos,
            decripcion = descripcion,
            photo = guidfoto,
            throwable = bool.Parse(throwable)
        };
        string cardjson = JsonUtility.ToJson(card);
        FirebaseDatabase.DefaultInstance.GetReference("Cards").Child(id.ToString()).SetRawJsonValueAsync(cardjson);
    }
}
