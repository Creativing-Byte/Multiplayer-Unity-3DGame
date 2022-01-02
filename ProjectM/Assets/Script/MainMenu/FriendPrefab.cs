using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FriendPrefab : MonoBehaviour
{
    public TextMeshProUGUI Nickname, Destreza, Team;
    public Image OnOff;

    public Sprite[] OfflineOnline;

    [HideInInspector]
    public Friend f = new Friend();

    GameObject Launcher;

    float seconds = 0;

    Friend result = new Friend();

    int dias, horas, minutos;

    string datenow;
    void Update()
    {
        Nickname.text = f.nickname;
        Nickname.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Nickname.text;
        Destreza.text = f.destreza.ToString();
        Destreza.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Destreza.text;
        Team.text = f.team;
        Team.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Team.text;
        if (Launcher == null)
        {
            try
            {
                Launcher = GameObject.FindGameObjectWithTag("Launcher");
            }
            catch (NullReferenceException)
            {
            }
        }

        seconds += Time.deltaTime;
        if (seconds > 5)
        {
            seconds = 0;
            StartCoroutine(LoadDate());
        }
        DateTime fecharegistro = DateTime.Now;
        try
        {
            fecharegistro = DateTime.ParseExact(f.ultcon, "MM'/'dd'/'yyyy' 'HH':'mm':'ss", new CultureInfo("es-VE"));
        }
        catch (ArgumentNullException)
        {
        }

        datenow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc).ToString("MM'/'dd'/'yyyy' 'HH':'mm':'ss");

        dias = (int)(((fecharegistro - DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)).TotalDays) * -1);
        horas = (int)(((fecharegistro - DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)).TotalHours) * -1);
        minutos = (int)(((fecharegistro - DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)).TotalMinutes) * -1);

        var ts = new TimeSpan(dias, horas, minutos, 0);

        int diaTransformado = ts.Days;
        int horaTransformada = ts.Hours;
        int minutoTransformado = ts.Minutes;

        if (diaTransformado > 0)
        {
            OnOff.sprite = OfflineOnline[0];
        }
        else if (diaTransformado == 0 && horaTransformada > 0)
        {
            OnOff.sprite = OfflineOnline[0];
        }
        else if (horaTransformada == 0 && minutoTransformado > 1)
        {
            OnOff.sprite = OfflineOnline[0];
        }
        else if (diaTransformado == 0 && horaTransformada == 0 && minutoTransformado < 2)
        {
            OnOff.sprite = OfflineOnline[1];
        }
    }
    IEnumerator LoadDate()
    {
        result = Launcher.GetComponent<UserDbInit>().LoadFriendData(f.Id);
        yield return new WaitForSeconds(3);
        f.ultcon = result.ultcon;
        yield return null;
    }
}