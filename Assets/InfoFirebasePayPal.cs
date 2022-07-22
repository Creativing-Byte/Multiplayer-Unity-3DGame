using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class InfoFirebasePayPal : MonoBehaviour
{
    public TMP_InputField fullName;
    public TMP_InputField adrress;
    public TMP_InputField PaypalEmail;
    public TMP_InputField moneyImput;
    public int money;

    //[System.Serializable]
    public struct Data
    {
        public string userName;
        public string password;

    }
    public Data dataWeb;
    //[System.Serializable]
    public struct DataMoney
    {
        public string email;
        public int money;
    }
    public DataMoney dataMoney;

    public string token;

    [ContextMenu("Escribir Simple")]
    public void EscibirSimple()
    {
        dataWeb.userName = "moneywar.admin";
        dataWeb.password = "Colombia2022*";
        dataMoney.email = PaypalEmail.text;
        dataMoney.money = money;
        //dataMoney.money = moneyImput;
        // aqui hay que hacer el if validando si la cantidad de monedas colocadas por el usuario en money es mayor o igual a la cantidad de monedas en firebase y listo --+
        /*if (money=>Firebase.usuario.moneda)
        {
            
        }
        else
        {
            PopupMessage;
        }*/
        StartCoroutine(CorrutinaEscibirSimple());
    }
    IEnumerator CorrutinaEscibirSimple()
    {
        string json = JsonUtility.ToJson(dataWeb);
        var req = new UnityWebRequest("http://moneywar.procotec.com.co/api/Authentication", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error While Sending: " + req.error);
        }
        else
        {
            token = req.downloadHandler.text;
            StartCoroutine(ChangeMony());
        }
    }
    IEnumerator ChangeMony()
    {
        string json = JsonUtility.ToJson(dataMoney);
        var req = new UnityWebRequest("http://moneywar.procotec.com.co/api/Transaction", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + token);
        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error While Sending: " + req.error);
        }
        else
        {
            Debug.Log("Received: " + req.downloadHandler.text);
        }
    }
}
