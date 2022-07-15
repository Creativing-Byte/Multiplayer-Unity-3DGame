using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatFriends : MonoBehaviour
{
    GameObject ChatPanel, FriendsPanel, SettingsPanel, Faq, LanguajePanel, Contraste;
    bool Open, OpenLanguaje;
    void Awake()
    {
        ChatPanel = GameObject.FindGameObjectWithTag("Chat");
        Contraste = GameObject.Find("Contraste");
        FriendsPanel = GameObject.FindGameObjectWithTag("Friends");
        SettingsPanel = GameObject.FindGameObjectWithTag("Settings");
        Faq = GameObject.FindGameObjectWithTag("Faq");
        LanguajePanel = GameObject.FindGameObjectWithTag("LanguajePanel");
    }
    public void OpenBox(int num)
    {
        if (Open)
        {
            switch (num)
            {
                case 0:
                    StartCoroutine(moveCoroutine(ChatPanel, 480));
                    Contraste.GetComponent<Image>().enabled = true;
                    break;
                case 1:
                    StartCoroutine(moveCoroutine(FriendsPanel, 480));
                    break;
                case 2:
                    StartCoroutine(moveCoroutine(SettingsPanel, 480));
                    break;
                case 3:
                    StartCoroutine(moveCoroutine(Faq, 480));
                    break;
            }
            Open = false;
        }
        else
        {
            switch (num)
            {
                case 0:
                    StartCoroutine(moveCoroutine(ChatPanel, 0));
                    Contraste.GetComponent<Image>().enabled = true;
                    break;
                case 1:
                    StartCoroutine(moveCoroutine(FriendsPanel, 0));
                    break;
                case 2:
                    StartCoroutine(moveCoroutine(SettingsPanel, 0));
                    break;
                case 3:
                    StartCoroutine(moveCoroutine(Faq, 0));
                    break;
            }
            Open = true;
        }
        if (num == 4)
        {
            if (OpenLanguaje)
            {
                StartCoroutine(moveCoroutine(LanguajePanel, 480));
                OpenLanguaje = false;
            }
            else
            {
                StartCoroutine(moveCoroutine(LanguajePanel, 0));
                OpenLanguaje = true;
            }
        }
    }
    public void ChangeLanguajeExt(int languaje)
    {
        AuthInit Launcher = GameObject.FindGameObjectWithTag("Launcher").GetComponent<AuthInit>();
        Launcher.ChangeLanguaje(languaje);
    }
    IEnumerator moveCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 40; i++)
        {
            Object.transform.localPosition = Vector3.Lerp(Object.transform.localPosition, new Vector3(x, 0, 0), 0.12f);
            yield return null;
        }
        if (Object.transform.localPosition != new Vector3(x, 0, 0))
        {
            Object.transform.localPosition = new Vector3(x, 0, 0);
        }
    }
}
