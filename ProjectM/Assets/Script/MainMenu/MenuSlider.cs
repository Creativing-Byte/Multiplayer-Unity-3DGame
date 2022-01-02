using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSlider : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    RectTransform Menu;
    int PosMenu = 0;

    public HudSlider HS;

    Vector3 lastMousePosition;

    public float time = 0.30f;

    bool Move = true;

    void Start()
    {
        Menu = GetComponent<RectTransform>();
        PosMenu = 0;
        time = 0.15f;
    }
    void Update()
    {
        if (Menu.localPosition.x > 960f)
        {
            Menu.localPosition = new Vector3(960, 0, 0);
        }
        else if ((Menu.localPosition.x < -960f))
        {
            Menu.localPosition = new Vector3(-960, 0, 0);
        }

        if (Move == true)
        {
            if (Menu.localPosition.x > 240f && Menu.localPosition.x < 720f)
            {
                ChangePosMenu(-1);
            }
            else if (Menu.localPosition.x < -240f && Menu.localPosition.x > -720f)
            {
                ChangePosMenu(1);
            }
            else if (Menu.localPosition.x < -720f)
            {
                ChangePosMenu(2);
            }
            else if (Menu.localPosition.x > 720f)
            {
                ChangePosMenu(-2);
            }
            else if (Menu.localPosition.x > -240f && Menu.localPosition.x < 240f)
            {
                ChangePosMenu(0);
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = Input.mousePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 diff = currentMousePosition - lastMousePosition;

        Vector3 newPosition = Menu.position + new Vector3(diff.x, 0, 0);
        Vector3 oldPos = Menu.position;
        Menu.position = newPosition;
        lastMousePosition = currentMousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Move = true;
        StartCoroutine(moveCoroutine());
    }
    IEnumerator moveCoroutine()
    {
        for (float i = 0; i < 40; i++)
        {
            if (PosMenu == 0)
            {
                Menu.localPosition = Vector3.Lerp(Menu.localPosition, new Vector3(0, 0, 0), time);
                if (Menu.localPosition.x == 0)
                {
                    break;
                }
            }
            else if (PosMenu == 1)
            {
                Menu.localPosition = Vector3.Lerp(Menu.localPosition, new Vector3(-480, 0, 0), time);
                if (Menu.localPosition.x == -480)
                {
                    break;
                }
            }
            else if (PosMenu == 2)
            {
                Menu.localPosition = Vector3.Lerp(Menu.localPosition, new Vector3(-960, 0, 0), time);
                if (Menu.localPosition.x == -960)
                {
                    break;
                }
            }
            else if (PosMenu == -1)
            {
                Menu.localPosition = Vector3.Lerp(Menu.localPosition, new Vector3(480, 0, 0), time);
                if (Menu.localPosition.x == 480)
                {
                    break;
                }
            }
            else if (PosMenu == -2)
            {
                Menu.localPosition = Vector3.Lerp(Menu.localPosition, new Vector3(960, 0, 0), time);
                if (Menu.localPosition.x == 960)
                {
                    break;
                }
            }
            yield return null;
        }
    }
    void ChangePosMenu(int i)
    {
        HS.Move(i);
        PosMenu = i;
        Move = false;
        StartCoroutine(moveCoroutine());
    }
}
