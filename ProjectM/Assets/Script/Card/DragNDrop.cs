using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static GameObject itemDragging;

    Vector3 startPosition;
    Transform startParent;
    Vector3 startScale;
    Camera Camera;
    GameObject Throw;
    GameObject Trigger;
    public Lanzamiento lanzador;

    public delegate void OnCardUsed(CardPlayer cardUsed);
    public static event OnCardUsed onCardUsed;


    void Awake()
    {
        Camera = GetComponentInParent<Camera>();
    }
    public void Start()
    {
        lanzador = gameObject.GetComponentInParent<Lanzamiento>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.localPosition;
        startParent = transform.parent;
        startScale = transform.localScale;
        transform.localScale *= 0.6f;
        Trigger = GameObject.FindGameObjectWithTag("Triggers" + GetComponent<CardPlayer>().team);

        for (int i = 0; i < Trigger.transform.childCount; i++)
        {
            if (!Trigger.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                Trigger.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GetComponent<CardPlayer>().Carta.name != "")
        {
            if (GetComponent<CardPlayer>().Reload.fillAmount <= 0 && GetComponent<CardPlayer>().elixir)
            {
                transform.position = Input.mousePosition;

                if (GetComponent<CardPlayer>().Carta.throwable)
                {
                    Trigger.transform.GetChild(5).gameObject.GetComponent<MeshRenderer>().enabled = true;
                    //Trigger.transform.GetChild(5).gameObject.transform.localPosition = new Vector3(Trigger.transform.GetChild(5).gameObject.transform.localPosition.x, Trigger.transform.GetChild(5).gameObject.transform.localPosition.y, 1f);
                }
                else
                {
                    Trigger.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                if (Trigger.transform.GetChild(2).GetComponent<Trigger>().Active == true && Trigger.transform.GetChild(4).GetComponent<Trigger>().Active == true)
                {
                    Trigger.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = Trigger.transform.GetChild(0).GetComponent<MeshRenderer>().enabled;
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetComponent<CardPlayer>().Reload.fillAmount <= 0 && GetComponent<CardPlayer>().elixir)
        {
            Throw = null;
            Trigger = GameObject.FindGameObjectWithTag("Triggers" + GetComponent<CardPlayer>().team);

            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].collider.gameObject.layer.Equals(11) && hits[i].collider.GetComponent<MeshRenderer>().enabled) 
                {
                    if (GetComponent<CardPlayer>().Reload.fillAmount <= 0)
                    {
                        onCardUsed?.Invoke(GetComponent<CardPlayer>());

                        GetComponent<CardPlayer>().poslanzamiento = hits[i].point;

                        if (GetComponent<CardPlayer>().Carta.throwable)
                        {

                            GameObject newThorw = Instantiate(GetComponent<CardPlayer>().ThrowableTrigger, /*GetComponent<CardPlayer>().poslanzamientolanzador.throwableTriggerPosition.position*/lanzador.throwableTriggerPosition.transform.position, Quaternion.identity);
                            GetComponent<CardPlayer>().Spawnthrowable(GameObject.FindGameObjectWithTag("Throwable" + GetComponent<CardPlayer>().team).transform.position, newThorw);
                        }
                        else 
                        {
                            GetComponent<CardPlayer>().Spawn();
                        }
                    }
                }
            }


            //Trigger.transform.GetChild(5).gameObject.transform.localPosition = new Vector3(Trigger.transform.GetChild(5).gameObject.transform.localPosition.x, Trigger.transform.GetChild(5).gameObject.transform.localPosition.y, 1.2f);
            Trigger.transform.GetChild(5).gameObject.GetComponent<MeshRenderer>().enabled = false;
            Trigger.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;

            if (Trigger.transform.GetChild(2).GetComponent<Trigger>().Active == true && Trigger.transform.GetChild(4).GetComponent<Trigger>().Active == true)
            {
                Trigger.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = Trigger.transform.GetChild(0).GetComponent<MeshRenderer>().enabled;
            }
        }

        //transform.localPosition = startPosition;
        transform.SetParent(startParent);
        transform.localScale = startScale;
    }
}
