using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudSlider : MonoBehaviour
{
    public Sprite[] FondosSprite = new Sprite[2]; //0 Selected, 1 UnSelected;

    public Image[] BackGroundOptions; //Imagenes q se cambiaran por los fondos segun su seleccion

    public GameObject[] Object;// los objectos q se moveran hacia arriba y se escalaran para resaltar su seleccion
    float time = 0.45f;
    public void Move(int PosMenu)
    {
        if (PosMenu == -2)
        {
            ChangeOptions(0, true);
        }
        else
        {
            ChangeOptions(0, false);
        }

        if (PosMenu == -1)
        {
            ChangeOptions(1, true);
        }
        else
        {
            ChangeOptions(1, false);
        }

        if (PosMenu == 0)
        {
            ChangeOptions(2, true);
        }
        else
        {
            ChangeOptions(2, false);
        }

        if (PosMenu == 1)
        {
            ChangeOptions(3, true);
        }
        else
        {
            ChangeOptions(3, false);
        }

        if (PosMenu == 2)
        {
            ChangeOptions(4, true);
        }
        else
        {
            ChangeOptions(4, false);
        }
    }
    void ChangeOptions(int Pos, bool change)
    {
        if (change)
        {
            BackGroundOptions[Pos].sprite = FondosSprite[0];
            StartCoroutine(scaleCoroutine(BackGroundOptions[Pos].gameObject, 1.05f, 1.15f));

            Object[Pos].GetComponentInChildren<Text>().enabled = true;
            Object[Pos].GetComponentInChildren<Text>().transform.GetChild(0).GetComponentInChildren<Text>().enabled = true;

            if (Pos == 2)
            {
                StartCoroutine(scaleCoroutine(Object[Pos], 1.04f, 1.04f));
            }

            StartCoroutine(moveCoroutine(Object[Pos], 0, 25));

            transform.GetChild(0).gameObject.transform.GetChild(Pos).GetComponent<Image>().enabled = true;
        }
        else
        {
            BackGroundOptions[Pos].sprite = FondosSprite[1];
            StartCoroutine(scaleCoroutine(BackGroundOptions[Pos].gameObject, 1, 1));

            Object[Pos].GetComponentInChildren<Text>().enabled = false;
            Object[Pos].GetComponentInChildren<Text>().transform.GetChild(0).GetComponentInChildren<Text>().enabled = false;

            if (Pos == 2)
            {
                StartCoroutine(scaleCoroutine(Object[Pos], 0.8f, 0.8f));
            }

            StartCoroutine(moveCoroutine(Object[Pos], 0, 0));

            transform.GetChild(0).gameObject.transform.GetChild(Pos).GetComponent<Image>().enabled = false;
        }
    }
    IEnumerator moveCoroutine(GameObject Object, float x, float y)
    {
        for (float i = 0; i < 20; i++)
        {
            Object.transform.localPosition = Vector3.Lerp(Object.transform.localPosition, new Vector3(x, y, 0), time);
            yield return null;
        }
        if (Object.transform.localPosition != new Vector3(x, y, 0))
        {
            Object.transform.localPosition = new Vector3(x, y, 0);
        }
    }
    IEnumerator scaleCoroutine(GameObject Object, float x, float y)
    {
        for (float i = 0; i < 20; i++)
        {
            Object.transform.localScale = Vector3.Lerp(Object.transform.localScale, new Vector2(x, y), time);
            yield return null;
        }
        if (Object.transform.localScale != new Vector3(x, y, 1))
        {
            Object.transform.localScale = new Vector3(x, y, 1);
        }
    }
}
