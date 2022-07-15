using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Idioma : MonoBehaviour
{
    public string[] Traduccion = new string[4];
    public bool Upper = true;

    private void Awake()
    {
        IdiomaManager.OnIdiomaSelect += UpdateIdioma;
        UpdateIdioma();
    }

    private void OnDisable() 
        => IdiomaManager.OnIdiomaSelect -= UpdateIdioma;

    void UpdateIdioma()
    {
        try
        {
            if (Upper)
            {
                GetComponent<Text>().text = Traduccion[IdiomaManager.SelectedIdioma].ToUpper();
            }
            else
            {
                GetComponent<Text>().text = Traduccion[IdiomaManager.SelectedIdioma];
            }

        }
        catch (NullReferenceException)
        {
            if (Upper)
            {
                try
                {
                    GetComponent<TextMeshProUGUI>().text = Traduccion[IdiomaManager.SelectedIdioma].ToUpper();
                }
                catch (NullReferenceException)
                {
                    GetComponent<TextMeshPro>().text = Traduccion[IdiomaManager.SelectedIdioma].ToUpper();
                }

            }
            else
            {
                try
                {
                    GetComponent<TextMeshProUGUI>().text = Traduccion[IdiomaManager.SelectedIdioma];
                }
                catch (NullReferenceException)
                {
                    GetComponent<TextMeshPro>().text = Traduccion[IdiomaManager.SelectedIdioma];
                }

            }

        }
    }
}

