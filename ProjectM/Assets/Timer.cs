using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text contador;
    public float time;
    public float tiempo;
    // Start is called before the first frame update
    void Start()
    {
        time = tiempo;
    }

    // Update is called once per frame
    void Update()
    {
        CalcularTiempo();
        if (tiempo <= 0)
        {
            contador.text = 0 + ":" + 0;
            tiempo = time;
            gameObject.SetActive(false);
        }
    }
    void CalcularTiempo()
    {
        tiempo -= Time.deltaTime;
        int minutos = (int)tiempo / 60;
        int segundos = (int)tiempo % 60;
        contador.text = segundos.ToString().PadLeft(2, '0');
    }
}
