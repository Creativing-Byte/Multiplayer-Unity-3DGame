using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    [System.Serializable]
    public struct Data
    {
        public string username;
        public string password;

    }
    public string user;
    public int monedas;
    public Data dataWeb;
    public string token;
    [ContextMenu("Leer Simple")]
    public void LeerSimple()
    {
        StartCoroutine(CorrutinaLeerSimple());
    }
    IEnumerator CorrutinaLeerSimple()
    {
        UnityWebRequest web = UnityWebRequest.Get("http://moneywar.procotec.com.co/api/Authentication");
        yield return web.SendWebRequest();
        Debug.Log(UnityWebRequest.Result.ProtocolError);
    }
    [ContextMenu("Escribir Simple")]
    public void EscibirSimple()
    {

        StartCoroutine(CorrutinaEscibirSimple());
    }
    IEnumerator CorrutinaEscibirSimple()
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
    }
}
