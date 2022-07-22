using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{

    /*IEnumerator CorrutinaEscibirSimple()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest web = UnityWebRequest.Put("http://moneywar.procotec.com.co/api/Authentication", "{\"userName\":\"moneywar.admin\",\"password\":\"Colombia2022*\"}");
        web.method = "POST";
        web.SetRequestHeader("Method", "POST");
        web.SetRequestHeader("Content-Type", "application/json");
        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(web.error);

        }
        else
        {
            token=web.downloadHandler.text;
            StartCoroutine(ChangeMony());
        }
    }
    IEnumerator ChangeMony()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest web = UnityWebRequest.Put("http://moneywar.procotec.com.co/api/Transaction", "{\"email\":\"user\",\"money\":\"50\"}");
        web.method = "POST";
        web.SetRequestHeader("Method", "POST");
        web.SetRequestHeader("Content-Type", "application/json");
        web.SetRequestHeader("Authorization", "Bearer " + token);
        yield return web.SendWebRequest();
        if (web.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(web.error);

        }
        else
        {
            Debug.Log(web.downloadHandler.text);
        }
    }*/
}
