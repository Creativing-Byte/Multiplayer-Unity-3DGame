using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MoveCourutine : MonoBehaviour
{
    float time = 0.15f;

    public GameObject[] Objecto;
    public bool Open;

    Vector2[] posInit = new Vector2[3];

    void OnEnable()
    {
        if (!Open)
        {
            Objecto[2].SetActive(true);
        }
        else
        {
            Objecto[2].SetActive(false);
        }
    }
    public void Move()
    {
        if (!Open)
        {
            Open = true;
            GetComponent<Image>().enabled = false;
            Objecto[2].SetActive(false);
            Objecto[0].GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
            posInit[0] = Objecto[0].GetComponent<RectTransform>().offsetMin;
            posInit[1] = Objecto[0].GetComponent<RectTransform>().offsetMax;
            posInit[2] = Objecto[1].transform.localPosition;

            if (Objecto[0].activeInHierarchy)
            {
                StartCoroutine(moveBottomCoroutine(Objecto[0], -365));
                StartCoroutine(moveTopCoroutine(Objecto[0], 34));
            }
            if (Objecto[1].activeInHierarchy)
            {
                StartCoroutine(moveCoroutine(Objecto[1], -180));
                Objecto[1].GetComponent<GridLayoutGroup>().padding.bottom = 10;
            }
        }
        else
        {
            Open = false;
            Objecto[2].SetActive(true);
            Objecto[0].GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Clamped;
            GetComponent<Image>().enabled = true;
            if (Objecto[0].activeInHierarchy)
            {
                StartCoroutine(ReturnMoveBottomCoroutine(Objecto[0], -50f));
                StartCoroutine(ReturnMoveTopCoroutine(Objecto[0], posInit[1]));
            }
            if (Objecto[1].activeInHierarchy)
            {
                StartCoroutine(ReturnMoveCoroutine(Objecto[1], -365));
                Objecto[1].GetComponent<GridLayoutGroup>().padding.bottom = 10;
            }
        }
    }
    IEnumerator moveBottomCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 40; i++)
        {
            Object.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(Object.GetComponent<RectTransform>().offsetMin, new Vector2(Object.GetComponent<RectTransform>().offsetMin.x, x), time);
            yield return null;
        }
    }
    IEnumerator ReturnMoveBottomCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 40; i++)
        {
            Object.GetComponent<RectTransform>().offsetMin = Vector2.Lerp(Object.GetComponent<RectTransform>().offsetMin, new Vector2(Object.GetComponent<RectTransform>().offsetMin.x, x), time);
            yield return null;
        }
    }
    IEnumerator moveTopCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 40; i++)
        {
            Object.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(Object.GetComponent<RectTransform>().offsetMax, new Vector2(Object.GetComponent<RectTransform>().offsetMax.x, x), time);
            yield return null;
        }
    }
    IEnumerator ReturnMoveTopCoroutine(GameObject Object, Vector2 x)
    {
        for (float i = 0; i < 30; i++)
        {
            Object.GetComponent<RectTransform>().offsetMax = Vector2.Lerp(Object.GetComponent<RectTransform>().offsetMax, posInit[1], time);
            yield return null;
        }
        StartCoroutine(ReturnMoveCoroutine(Objecto[1], -400));
    }
    IEnumerator moveCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 40; i++)
        {
            Object.transform.localPosition = Vector2.Lerp(Object.transform.localPosition, new Vector2(Object.transform.localPosition.x, x), time);
            yield return null;
        }
    }
    IEnumerator ReturnMoveCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 30; i++)
        {
            Object.transform.localPosition = Vector3.Lerp(Object.transform.localPosition, new Vector3(Object.transform.localPosition.x, x, Object.transform.localPosition.z), time);
            yield return null;
        }

    }
}
