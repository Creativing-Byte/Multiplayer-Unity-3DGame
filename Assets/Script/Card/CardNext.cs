using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardNext : MonoBehaviour
{
    [HideInInspector]
    public Card Carta = new Card();
    public TextMeshProUGUI Amount;
    GameObject Launcher;
    void Awake()
    {
        Launcher = GameObject.FindWithTag("Launcher");      
    }
    void Update()
    {
        if (Launcher.GetComponent<PhotonInit>().StartPlayerToPlay == true)
        {
            if (Carta == null)
            {
                GetComponent<Image>().enabled = false;
            }
            else
            {
                GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }
        if (GetComponent<Image>().sprite == null && gameObject.activeInHierarchy && Carta != null)
        {
            LoadImage();
        }
    }
    public void LoadImage()
    {
        StartCoroutine(ImageLoader());
        try
        {
            Amount.text = Carta.level.ToString();
        }
        catch (System.NullReferenceException)
        {
        }
    }
    IEnumerator ImageLoader()
    {
        try
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/" + Carta.photo);
        }
        catch (NullReferenceException)
        {
        }       
        yield return null;
    }
    public void DeleteCard()
    {
        Carta = null;
        GetComponent<Image>().sprite = null;
    }
}
