using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Sprite[] OnOff = new Sprite[2];
    public Sprite[] Mute = new Sprite[2];
    public Image[] Options = new Image[4];
    void Start()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 1)
        {
            PlayerPrefs.SetInt("Music", 1);
            Options[0].sprite = OnOff[0];
            Options[0].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[0].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Options[0].sprite = OnOff[1];
            Options[0].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[0].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        if (PlayerPrefs.GetInt("SFX", 1) == 1)
        {
            PlayerPrefs.SetInt("SFX", 1);
            Options[1].sprite = OnOff[0];
            Options[1].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[1].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Options[1].sprite = OnOff[1];
            Options[1].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[1].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        if (PlayerPrefs.GetInt("Push", 1) == 1)
        {
            PlayerPrefs.SetInt("Push", 1);
            Options[2].sprite = OnOff[0];
            Options[2].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[2].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Options[2].sprite = OnOff[1];
            Options[2].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[2].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        if (PlayerPrefs.GetInt("Kids", 0) == 0)
        {
            PlayerPrefs.SetInt("Kids", 0);
            Options[3].sprite = OnOff[1];
            Options[3].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[3].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            Options[3].sprite = OnOff[0];
            Options[3].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[3].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
    }
    public void ChangeButtons(int num)
    {
        if (num == 0)
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Music", 0);
            }
        }
        if (num == 1)
        {
            if (PlayerPrefs.GetInt("SFX") == 0)
            {
                PlayerPrefs.SetInt("SFX", 1);
            }
            else
            {
                PlayerPrefs.SetInt("SFX", 0);
            }
        }
        if (num == 2)
        {
            if (PlayerPrefs.GetInt("Push") == 0)
            {
                PlayerPrefs.SetInt("Push", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Push", 0);
            }
        }
        if (num == 3)
        {
            if (PlayerPrefs.GetInt("Kids") == 0)
            {
                PlayerPrefs.SetInt("Kids", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Kids", 0);
            }
        }

        if (PlayerPrefs.GetInt("Music", 1) == 1)
        {
            Options[0].sprite = OnOff[0];
            Options[0].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[0].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Options[0].sprite = OnOff[1];
            Options[0].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[0].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        if (PlayerPrefs.GetInt("SFX", 1) == 1)
        {
            Options[1].sprite = OnOff[0];
            Options[1].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[1].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Options[1].sprite = OnOff[1];
            Options[1].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[1].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        if (PlayerPrefs.GetInt("Push", 1) == 1)
        {
            Options[2].sprite = OnOff[0];
            Options[2].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[2].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
        else
        {
            Options[2].sprite = OnOff[1];
            Options[2].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[2].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        if (PlayerPrefs.GetInt("Kids", 0) == 0)
        {
            Options[3].sprite = OnOff[1];
            Options[3].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OFF";
            Options[3].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "OFF";
        }
        else
        {
            Options[3].sprite = OnOff[0];
            Options[3].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ON";
            Options[3].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "ON";
        }
    }
}
