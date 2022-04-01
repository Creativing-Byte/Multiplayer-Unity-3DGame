using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class CardPlayer : MonoBehaviour
{
    public GameObject vfxSpawn;

    [HideInInspector]
    public Card Carta = new Card();
    [HideInInspector]
    public bool start, elixir;

    [HideInInspector]
    public Vector3 poslanzamiento;

    [HideInInspector]
    public GameObject Launcher, Portal;

    public string team;
    [HideInInspector]
    public Image Reload;

    GameObject Marco;

    [HideInInspector]
    public float secondsCounter = 0;
    Camera Camera;
    PhotonView myview;

    public GameObject[] CartaPrebafs;

    public AudioClip SpawnSfxClip;

    public GameObject ThrowableTrigger;

    public bool canLoad = true;

    private Vector3 endpoint;
    private Vector3 endScale;

    int spawn;
    void Awake()
    {
        try
        {
            myview = GetComponent<PhotonView>();
        }
        catch (NullReferenceException)
        {
        }

        Launcher = GameObject.FindWithTag("Launcher");
        Camera = GetComponentInParent<Camera>();
        Marco = transform.parent.gameObject;
        Reload = transform.GetChild(0).GetComponent<Image>();
      

        endpoint = transform.position;
        endScale = transform.localScale;
        canLoad = true;
    }
    void Update()
    {
        if (start)
        {
            if (Carta == null)
            {
                GetComponent<Image>().enabled = false;
                Marco.GetComponent<Image>().enabled = false;
            }
            else
            {
                GetComponent<Image>().enabled = true;
                Marco.GetComponent<Image>().enabled = true;
            }

            if (Carta != null)
            {
                try
                {
                    if (GetComponentInParent<PowerCardSystem>().valueAct >= Carta.level)
                    {
                        elixir = true;
                    }
                    else
                    {
                        elixir = false;
                    }
                    secondsCounter = Carta.time;

                    if (canLoad)
                    {
                        Reload.fillAmount -= 1.0f / secondsCounter * Time.deltaTime;
                    }
                }
                catch (NullReferenceException)
                {
                }
            }
            if (GetComponent<Image>().sprite == null && gameObject.activeInHierarchy)
            {
                Reload.fillAmount = 1;
                LoadImage();
            }
            try
            {
                transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Carta.level.ToString();
            }
            catch (System.NullReferenceException)
            {
            }           
        }
        else
        {
            GetComponent<Image>().enabled = true;
            try
            {
                transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "0";
            }
            catch (System.NullReferenceException)
            {
            }            
        }
    }
    public void LoadData()
    {
        start = true;
    }
    public void LoadImage()
    {
        StartCoroutine(ImageLoader());
    }
    IEnumerator ImageLoader()
    {
        try
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/" + Carta.photo);
        }
        catch (NullReferenceException)
        {
        }
        yield return null;
    }

    public void StartDrawing(Vector3 startPoint, Vector3 scale)
    {
        canLoad = false;
        transform.position = Vector3.zero;
        transform.localScale = scale;
        GetComponent<Image>().enabled = false;
    }

    public bool DrawAnimation()
    {
        transform.position = Vector3.Lerp(transform.position, endpoint, Time.deltaTime * 3);
        transform.localScale = Vector3.Lerp(transform.localScale, endScale, Time.deltaTime * 2.5f);

        if (Vector3.Distance(transform.position, endpoint) <= 0.1f)
        {
            transform.position = endpoint;
            transform.localScale = endScale;
            canLoad = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Spawn(GameObject Throwable)
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Carta.Prefabs != "")
            {
                GameObject Spawn;
                SpawnSfx();

                for (int i = 0; i < CartaPrebafs.Length; i++)
                {
                    if (CartaPrebafs[i].name == Carta.Prefabs)
                    {
                        Spawn = Instantiate(CartaPrebafs[i], poslanzamiento, Quaternion.identity);

                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                        {
                            if (!m.enabled)
                                m.enabled = true;
                        }

                        LoadStats(Spawn);
                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level*10);
                        spawn -= 1;
                        if (spawn <= 0)
                        {
                            DeleteCard();
                        }
                        else
                        {
                            Reload.fillAmount = 1;
                            secondsCounter = Carta.time;
                        }
                        break;
                    }
                }
            }
        }
        else
        {
            if (myview != null)
            {
                if (!myview.IsMine)
                {
                    return;
                }
                if (Carta.Prefabs != "")
                {
                    if (Carta.throwable == false)
                    {
                        GameObject Spawn;
                        SpawnSfx();
                        if (team == "Red")
                        {
                            Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.Euler(0, 180, 0));
                        }
                        else
                        {
                            Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.identity);
                        }
                        print(team);
                        vfxSpawn = PhotonNetwork.Instantiate("spawn", poslanzamiento, Quaternion.Euler(90, 0, 0));

                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                        {

                            if (!m.enabled)
                                m.enabled = true;
                        }

                        LoadStats(Spawn);
                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                        Carta.spawn -= 1;
                        if (Carta.spawn <= 0)
                        {
                            print("yes");
                            DeleteCard();
                        }
                        else
                        {
                            Reload.fillAmount = 1;
                            secondsCounter = Carta.time;
                        }
                    }

                    else if (Carta.throwable == true)
                    {
                        if (Carta.Prefabs != "")
                        {
                            GameObject Spawn;
                            SpawnSfx();
                            switch (Carta.Prefabs)
                            {
                                case "Velociraptor":
                                    if (poslanzamiento != Vector3.zero)
                                    {
                                        Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.identity);

                                        Throwable.GetComponent<ThrowableTrigger>().ObjectName = Spawn.gameObject.name;

                                        //Spawn.GetComponent<Velociraptors>().PosLanzamiento = poslanzamiento;
                                        //poslanzamiento = Vector3.zero;

                                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                                        {
                                            if (!m.enabled)
                                                m.enabled = true;
                                        }
                                        LoadStats(Spawn);
                                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                                        Carta.spawn -= 1;
                                        if (Carta.spawn <= 0)
                                        {
                                            print("yesasdsd");
                                            DeleteCard();
                                            print("yesasdsdasdasd" +
                                                "");
                                        }
                                        else
                                        {
                                            Reload.fillAmount = 1;
                                            secondsCounter = Carta.time;
                                        }
                                    }
                                    break;
                                case "ZarigueyaPrefacInstancia":
                                    if (poslanzamiento != Vector3.zero)
                                    {
                                        Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.identity);

                                        Throwable.GetComponent<ThrowableTrigger>().ObjectName = Spawn.gameObject.name;

                                        Spawn.GetComponent<ZarigueyaPrefac>().PosLanzamiento = poslanzamiento;
                                        poslanzamiento = Vector3.zero;

                                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                                        {
                                            print(1);
                                            if (!m.enabled)
                                                m.enabled = true;
                                        }
                                        LoadStats(Spawn);
                                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                                        Carta.spawn -= 1;
                                        if (Carta.spawn <= 0)
                                        {
                                            print("yes");
                                            DeleteCard();
                                        }
                                        else
                                        {
                                            Reload.fillAmount = 1;
                                            secondsCounter = Carta.time;
                                        }
                                    }
                                    break;
                                case "FireballLanzador":
                                    if (poslanzamiento != Vector3.zero)
                                    {
                                        Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.identity);

                                        Throwable.GetComponent<ThrowableTrigger>().ObjectName = Spawn.gameObject.name;

                                        //Spawn.GetComponent<FireballPrefac>().PosLanzamiento = poslanzamiento;
                                        poslanzamiento = Vector3.zero;

                                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                                        {
                                            if (!m.enabled)
                                                m.enabled = true;
                                        }
                                        LoadStats(Spawn);
                                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                                        Carta.spawn -= 1;
                                        if (Carta.spawn <= 0)
                                        {
                                            print("yes");
                                            DeleteCard();
                                        }
                                        else
                                        {
                                            Reload.fillAmount = 1;
                                            secondsCounter = Carta.time;
                                        }
                                    }
                                    break;
                                case "HachasPrefac":
                                    if (poslanzamiento != Vector3.zero)
                                    {
                                        Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.identity);

                                        Throwable.GetComponent<ThrowableTrigger>().ObjectName = Spawn.gameObject.name;

                                        Spawn.GetComponent<AxesPrefac>().PosLanzamiento = poslanzamiento;
                                        poslanzamiento = Vector3.zero;

                                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                                        {
                                            if (!m.enabled)
                                                m.enabled = true;
                                        }
                                        LoadStats(Spawn);
                                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                                        Carta.spawn -= 1;
                                        if (Carta.spawn <= 0)
                                        {
                                            print("yes");
                                            DeleteCard();
                                        }
                                        else
                                        {
                                            Reload.fillAmount = 1;
                                            secondsCounter = Carta.time;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    /*if (Carta.Prefabs != "")
                    {
                        GameObject Spawn;
                        SpawnSfx();
                        if (team=="Red")
                        {
                            Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.Euler(0,180,0));

                        }
                        else
                        {
                            Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.identity);
                        }
                        print(team);
                        vfxSpawn = PhotonNetwork.Instantiate("spawn", poslanzamiento, Quaternion.Euler(90, 0, 0));

                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                        {

                            if (!m.enabled)
                                m.enabled = true;
                        }

                        LoadStats(Spawn);
                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level*10);
                        Carta.spawn -= 1;
                        if (Carta.spawn <= 0)
                        {
                            print("yes");
                            DeleteCard();
                        }
                        else
                        {
                            Reload.fillAmount = 1;
                            secondsCounter = Carta.time;
                        }*/

                }
                else if (Carta.Prefabs == "DragonGigante")
                {
                    if (Carta.throwable == false)
                    {
                        GameObject Spawn;
                        SpawnSfx();
                        if (team == "Red")
                        {
                            Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.Euler(-90, 180, 0));
                        }
                        else
                        {
                            Spawn = PhotonNetwork.Instantiate(Carta.Prefabs, poslanzamiento, Quaternion.Euler(-90, 0, 0));
                        }
                        print(team);
                        vfxSpawn = PhotonNetwork.Instantiate("spawn", poslanzamiento, Quaternion.Euler(90, 0, 0));

                        foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                        {

                            if (!m.enabled)
                                m.enabled = true;
                        }

                        LoadStats(Spawn);
                        GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                        Carta.spawn -= 1;
                        if (Carta.spawn <= 0)
                        {
                            print("yes");
                            DeleteCard();
                        }
                        else
                        {
                            Reload.fillAmount = 1;
                            secondsCounter = Carta.time;
                        }
                    }

                }
            }
        }
    }
    public void Spawnthrowable( GameObject Throwable)
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Carta.Prefabs != "")
            {
                GameObject Spawn;
                SpawnSfx();
                switch (Carta.Prefabs)
                {
                    case "Velociraptor":
                        for (int i = 0; i < CartaPrebafs.Length; i++)
                        {
                            if (CartaPrebafs[i].name == Carta.Prefabs)
                            {
                                if (poslanzamiento != Vector3.zero)
                                {
                                    /*Spawn = Instantiate(CartaPrebafs[i], PosLanzamiento, Quaternion.identity);
                                    //Throwable.GetComponent<ThrowableTrigger>().ObjectName = Spawn.gameObject.name;

                                    Spawn.GetComponent<Velociraptors>().PosLanzamiento = poslanzamiento;
                                    poslanzamiento = Vector3.zero;
                                    foreach (MonoBehaviour m in Spawn.GetComponents<MonoBehaviour>())
                                    {
                                        if (!m.enabled)
                                            m.enabled = true;
                                    }
                                    LoadStats(Spawn);
                                    GetComponentInParent<PowerCardSystem>().Spawn(Carta.level * 10);
                                    spawn -= 1;
                                    if (spawn <= 0)
                                    {
                                        DeleteCard();
                                    }
                                    else
                                    {
                                        Reload.fillAmount = 1;
                                        secondsCounter = Carta.time;
                                    }*/
                                }
                                break;
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            if (myview != null)
            {
                if (!myview.IsMine)
                {
                    return;
                }

                
            }
        }
    }
    void LoadStats(GameObject Spawn)
    {
        if (Carta.Prefabs == "MiniKnight")
        {
            Spawn.GetComponent<LanzadorMiniK>().Stats.team = team;
            Spawn.GetComponent<LanzadorMiniK>().Stats.vidamax = Carta.vida;
            Spawn.GetComponent<LanzadorMiniK>().Stats.vidacurrent = Carta.vida;
            Spawn.GetComponent<LanzadorMiniK>().Stats.ataque = Carta.ataque;
            Spawn.GetComponent<LanzadorMiniK>().Stats.velocidad = Carta.velocidad;
            Spawn.GetComponent<LanzadorMiniK>().Stats.vataque = Carta.vataque;
            Spawn.GetComponent<LanzadorMiniK>().Stats.Range = Carta.range;
        }
        if (Carta.Prefabs == "Velociraptor")
        {
            Spawn.GetComponent<Velociraptors>().Stats.team = team;
            Spawn.GetComponent<Velociraptors>().Stats.vidamax = Carta.vida;
            Spawn.GetComponent<Velociraptors>().Stats.vidacurrent = Carta.vida;
            Spawn.GetComponent<Velociraptors>().Stats.ataque = Carta.ataque;
            Spawn.GetComponent<Velociraptors>().Stats.velocidad = Carta.velocidad;
            Spawn.GetComponent<Velociraptors>().Stats.vataque = Carta.vataque;
            Spawn.GetComponent<Velociraptors>().Stats.Range = Carta.range;
        }
        if (Carta.Prefabs == "ZarigueyaPrefacInstancia")
        {
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.team = team;
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.vidamax = Carta.vida;
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.vidacurrent = Carta.vida;
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.ataque = Carta.ataque;
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.velocidad = Carta.velocidad;
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.vataque = Carta.vataque;
            Spawn.GetComponent<ZarigueyaPrefac>().Stats.Range = Carta.range;
        }
        if (Carta.Prefabs == "FireballLanzador")
        {
            Spawn.GetComponent<FireballInsta>().Stats.team = team;
            Spawn.GetComponent<FireballInsta>().Stats.vidamax = Carta.vida;
            Spawn.GetComponent<FireballInsta>().Stats.vidacurrent = Carta.vida;
            Spawn.GetComponent<FireballInsta>().Stats.ataque = Carta.ataque;
            Spawn.GetComponent<FireballInsta>().Stats.velocidad = Carta.velocidad;
            Spawn.GetComponent<FireballInsta>().Stats.vataque = Carta.vataque;
            Spawn.GetComponent<FireballInsta>().Stats.Range = Carta.range;
        }
        if (Carta.Prefabs == "HachasPrefac")
        {
            Spawn.GetComponent<AxesPrefac>().Stats.team = team;
            Spawn.GetComponent<AxesPrefac>().Stats.vidamax = Carta.vida;
            Spawn.GetComponent<AxesPrefac>().Stats.vidacurrent = Carta.vida;
            Spawn.GetComponent<AxesPrefac>().Stats.ataque = Carta.ataque;
            Spawn.GetComponent<AxesPrefac>().Stats.velocidad = Carta.velocidad;
            Spawn.GetComponent<AxesPrefac>().Stats.vataque = Carta.vataque;
            Spawn.GetComponent<AxesPrefac>().Stats.Range = Carta.range;
        }
        else
        {
            Spawn.GetComponent<Player>().prefac = Carta.Prefabs;
            Spawn.GetComponent<Player>().Stats.team = team;
            Spawn.GetComponent<Player>().Stats.vidamax = Carta.vida;
            Spawn.GetComponent<Player>().Stats.vidacurrent = Carta.vida;
            Spawn.GetComponent<Player>().Stats.ataque = Carta.ataque;
            Spawn.GetComponent<Player>().Stats.velocidad = Carta.velocidad;
            Spawn.GetComponent<Player>().Stats.vataque = Carta.vataque;
            Spawn.GetComponent<Player>().Stats.Range = Carta.range;
        }
    }
    public void DeleteCard()
    {
        GetComponentInParent<CardLoaded>().Cartas.Add(Carta);
        Carta = null;
        GetComponent<Image>().sprite = null;
        Reload.fillAmount = 0;
        CardLoaded.LanzarCarta = false;
        start = false;
        secondsCounter = 0;
    }
    void SpawnSfx()
    {
        GetComponent<AudioSource>().clip = SpawnSfxClip;
        GetComponent<AudioSource>().Play();
    }
}