using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerCardSystem : MonoBehaviour
{
    public Image Medidor;
public TextMeshProUGUI Amount;
    int maxValue = 100;
    [HideInInspector]
    public int valueAct;

    float secondsCounter;
    float secondsToCount = 0.3f;

    float Range;
    public Gradient myGradient;

    void Awake()
    {
        Medidor.fillAmount = 0;
    }

    void Update()
    {
        Range = 1.0f / maxValue * valueAct;
        Medidor.fillAmount = Range;

        if (valueAct < maxValue)
        {
            secondsCounter += Time.deltaTime;
        }

        //Medidor.color = myGradient.Evaluate(Range);

        if (valueAct < maxValue)
        {
            if (secondsCounter >= secondsToCount)
            {
                secondsCounter = 0;
                valueAct += 1;
            }
        }

        if (valueAct >= maxValue)
        {
            secondsCounter = -0.12f;
        }
        if (valueAct < 0)
        {
            valueAct = 0;
        }
        Amount.text = ((int)valueAct/10).ToString();
    }

    public void Spawn(int level)
    {
        secondsCounter = -0.12f;
        valueAct -= level; 
        Range = 1.0f / maxValue * valueAct;
        Medidor.fillAmount = Range;   
    }
}
