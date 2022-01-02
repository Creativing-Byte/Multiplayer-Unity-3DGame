using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    public static PopupMessage instance;

    [SerializeField] private GameObject popupMessage;
    [SerializeField] private TMP_Text txtMessage;

    private Coroutine cor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start() => popupMessage.transform.localScale = new Vector3(1, 0, 1);

    public void Show(string message)
    {
        if (cor == null)
            cor = StartCoroutine(Message(message));
    }

    private IEnumerator Message(string message)
    {
        float scale = 0;
        txtMessage.text = message;

        while (scale < 1)
        {
            popupMessage.transform.localScale = new Vector3(1, scale += 0.1f, 1);
            yield return new WaitForEndOfFrame();    
        }

        yield return new WaitForSeconds(2);

        while (scale > 0)
        {
            popupMessage.transform.localScale = new Vector3(1, scale -= 0.1f, 1);
            yield return new WaitForEndOfFrame();
        }

        popupMessage.transform.localScale = new Vector3(1, 0, 1);
        cor = null;
    }
}
