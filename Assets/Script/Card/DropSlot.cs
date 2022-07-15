using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject item;

    public Image imagen;
    [HideInInspector]
    public bool enter;
    public GameObject[] GInterface;
    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            if (GetComponent<CardPlayer>().Carta == null)
            {
                GetComponent<CardPlayer>().Carta = DragNDrop.itemDragging.GetComponent<CardNext>().Carta;
                GetComponent<CardPlayer>().LoadData();
                DragNDrop.itemDragging.GetComponent<CardNext>().DeleteCard();
            }
        }
    }
    void Update()
    {
        if (item != null)
        {
            item = null;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        try
        {
            if (GetComponent<CardPlayer>().start == false && DragNDrop.itemDragging.GetComponent<Image>() != null)
            {
                enter = true;
                var tempColor = imagen.color;
                tempColor.a = 1f;
                imagen.color = tempColor;
                imagen.sprite = DragNDrop.itemDragging.GetComponent<Image>().sprite;
            }
        }
        catch (NullReferenceException)
        {
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (enter)
        {
            try
            {
                imagen.sprite = null;
                var tempColor = imagen.color;
                tempColor.a = 0f;
                imagen.color = tempColor;
                enter = false;
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}
