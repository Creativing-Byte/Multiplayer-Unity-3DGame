using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [HideInInspector] public Card card;
    [HideInInspector] public Image image;
    [HideInInspector] public Button button;
    [HideInInspector] public Text Type;

    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void Load(Card cardRef, Sprite sprite, UnityAction callingFunction)
    {
        if (image == null) 
            image = GetComponent<Image>();
        if (button == null) 
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();

        card = cardRef;
        image.sprite = sprite;
        image.preserveAspect = true;
        button.onClick.AddListener(callingFunction);
        button.interactable = true;
    }

    public void Load(Card cardRef)
    {
        if (image == null)
            image = GetComponent<Image>();
        if (button == null)
            button = GetComponent<Button>();

        card = cardRef;
        image.sprite = Resources.Load<Sprite>($"Cards/{cardRef.photo}");
        image.preserveAspect = true;
        button.interactable = true;
    }
}
