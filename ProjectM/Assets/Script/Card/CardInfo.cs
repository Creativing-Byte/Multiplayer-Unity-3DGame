using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public int idpos;
    public bool deckmanager;
    public Card Carta = new Card();
    void Update()
    {
        if (transform.GetChild(0).gameObject.GetComponent<Image>().sprite == null)
        {
            var tempColor = transform.GetChild(0).gameObject.GetComponent<Image>().color;
            tempColor.a = 0f;
            transform.GetChild(0).gameObject.GetComponent<Image>().color = tempColor;
        }
        else
        {
            var tempColor = transform.GetChild(0).gameObject.GetComponent<Image>().color;
            tempColor.a = 1f;
            transform.GetChild(0).gameObject.GetComponent<Image>().color = tempColor;
        }
        if (Carta != null)
        {
            transform.GetChild(1).gameObject.GetComponent<Text>().text = Carta.spawn.ToString();
            transform.GetChild(2).gameObject.GetComponent<Text>().text = Carta.level.ToString();
        }
    }
    public void LoadImage()
    {
        StartCoroutine(ImageLoader());
    }
    IEnumerator ImageLoader()
    {
        transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/" + Carta.photo);
        yield return null;
    }
}
