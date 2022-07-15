using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragPanel : MonoBehaviour, IEndDragHandler
{
    float time = 0.15f;

    public GameObject move;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetComponent<ScrollRect>().content.gameObject.transform.localPosition.y > -100f && move.GetComponent<MoveCourutine>().Open)
        {
            move.GetComponent<MoveCourutine>().Move();
        }
    }
    IEnumerator ReturnMoveCoroutine(GameObject Object, float x)
    {
        for (float i = 0; i < 20; i++)
        {
            Object.transform.localPosition = Vector3.Lerp(Object.transform.localPosition, new Vector3(Object.transform.localPosition.x, x, Object.transform.localPosition.z), time);
            yield return null;
        }

    }
}
