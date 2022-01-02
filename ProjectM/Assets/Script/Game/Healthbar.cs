using UnityEngine.UI;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Sprite healthBarRed;
    [SerializeField] private Image healthbarBack;
    [SerializeField] private Image healthbarFill;
    [SerializeField] private float healthMax = 100;
    [SerializeField] private float startingHealth = 100;

    private void Awake()
    {
        var p1 = GameObject.Find("P1");
        var p2 = GameObject.Find("P2");

        if (p1 != null)
            transform.GetComponent<Canvas>().worldCamera = p1.transform.parent.GetComponent<Camera>();

        if (p2 != null)
            transform.GetComponent<Canvas>().worldCamera = p2.transform.parent.GetComponent<Camera>();
    }

    public float Fill
    {
        get => healthbarFill.fillAmount;
        set
        {
            float hp = Mathf.Clamp(value, 0, 1);
            healthbarFill.fillAmount = hp;
        }
    }

    private void Start() => Set(startingHealth);

    public void SetColor(bool isMine)
    {
        if (isMine)
        {
            if (PhotonInit.MyTeam.ToLower() == "red")
                healthbarFill.sprite = healthBarRed;
        }
        else
        {
            if (PhotonInit.MyTeam.ToLower() == "blue")
                healthbarFill.sprite = healthBarRed;
        }
    }    

    public void Set(float health)
    {
        float hp = Mathf.Clamp(health, 0, healthMax);
        healthbarFill.fillAmount = healthMax / hp;
    }

    public void Damage(float amount) => healthbarFill.fillAmount -= amount / healthMax;
    
    public void Heal(float amount) => healthbarFill.fillAmount += amount / healthMax;
}
