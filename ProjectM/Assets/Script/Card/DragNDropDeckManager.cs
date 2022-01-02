using UnityEngine;
using UnityEngine.UI;

public class DragNDropDeckManager : MonoBehaviour
{
    public static GameObject itemDragging;
    Vector3 startPosition;
    Transform startParent;
    public Transform dragParent;
    Vector3 startScale;
    public Vector3 posFix;

    GameObject CartaPrefabs1;

    public Image Fondo;
    public void itemdragging()
    {
        if (itemDragging == null)
        {
            itemDragging = gameObject;
            Fondo.enabled = true;
        }
        else
        {
            GetComponent<DropSlotDeckManager>().itemdraggingdrop(itemDragging);
            itemDragging = null;
            Fondo.enabled = false;
        }
    }
    public void nullable()
    {
        itemDragging = null;
        Fondo.enabled = false;
    }
}
