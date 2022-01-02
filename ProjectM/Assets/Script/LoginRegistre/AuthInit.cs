using Firebase;
using Firebase.Auth;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthInit : MonoBehaviour
{
    //----------------------------------------------------------------------------------
    protected FirebaseAuth auth;
    protected FirebaseUser user;
    bool signedIn;

    public GameObject LanguagePanel, LoginPanel, Background;

    public FirebaseUser User => user;

    public void InitializeFirebase() // 4) Funcion inicial en el flujo AuthInit; Configura y conecta con firebase y sus servicios : Ramifica en AuthStateChanged();
    {
        //await FirebaseApp.CheckAndFixDependenciesAsync();

        //la variable auth se llena con los valores por defecto obtenido en json de firebase
        auth = FirebaseAuth.DefaultInstance;

        //FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri("https://projectm-coffetimestudio.firebaseio.com/");

        auth.StateChanged += AuthStateChanged;
    }
    void AuthStateChanged(object sender, EventArgs eventArgs) // 5) Funcion de verificacion de usuario activo; Verifica un usuario activo : Ramifica en InicializarBd() 6, Ramifica en LoginRegistre[0].SetActive(true) 5.B;
    {
        //verificamos al usuario actual
        if (auth.CurrentUser != user)
        {
            signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            //si el usuario cierra session
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            //si el usuario esta con una session iniciada
            if (signedIn)
            {
                GetComponent<UserDbInit>().InicializarBd(user);
            }
        }
        //si no hay usuario con session abierta, creamo un usuario anonimo
        if (auth.CurrentUser == null)
        {
            if (PlayerPrefs.GetInt("IdiomaConfirm") == 1)
            {
                Background.SetActive(false);
                LoginPanel.SetActive(true);
                LanguagePanel.SetActive(true);
                ChangeLanguaje(PlayerPrefs.GetInt("Idioma"));
                LanguagePanel.SetActive(false);
            }
            else
            {
                Background.SetActive(false);
                LanguagePanel.SetActive(true);
                LoginPanel.SetActive(true);
                ChangeLanguaje(0);
            }
        }
    }
    void OnDestroy()
    {
        try
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
        catch (NullReferenceException)
        {
        }
    }
    public void Login() // 5.B.A) Funcion de inicio de session de usuario : Ramifica en Login();
    {
        FindObjectOfType<LoginRegistre>().Login(auth);
    }
    public void Registre() // 5.B.B) Funcion de registro de usuario : Ramifica en Registre();
    {
        FindObjectOfType<LoginRegistre>().Registre(auth);
    }
    public void RecoveryPassword() // 5.B.C) Funcion de recuperacion de contraseña : Ramifica en Recovery();
    {
        FindObjectOfType<LoginRegistre>().RecoveryPassword(auth);
    }
    public void LogOut()
    {
        try
        {
            auth.SignOut();
        }
        catch (NullReferenceException)
        {
        }
    }
    public void ExitApp()
    {
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            Application.Quit();
        }
    }
    public void ChangeLanguaje(int langueaje)
    {
        PlayerPrefs.SetInt("Idioma", langueaje);
        IdiomaManager.IdiomaUpdated();
        PlayerPrefs.SetInt("IdiomaConfirm", 1);

        GameObject Languaje = GameObject.FindGameObjectWithTag("Languaje"), LanguajeCurrent = GameObject.FindGameObjectWithTag("LanguajeCurrent");
        if (PlayerPrefs.GetInt("Idioma") == 0)
        {
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                if (Languaje.transform.GetChild(i).gameObject.name == "Español")
                {
                    LanguajeCurrent.transform.GetChild(0).gameObject.transform.SetParent(Languaje.transform);
                    Languaje.transform.GetChild(i).gameObject.transform.SetParent(LanguajeCurrent.transform);
                }
            }
            LanguajeCurrent.transform.GetChild(0).gameObject.GetComponentInChildren<Button>().interactable = false;
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                Languaje.transform.GetChild(i).gameObject.GetComponentInChildren<Button>().interactable = true;
            }
        }
        if (PlayerPrefs.GetInt("Idioma") == 1)
        {
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                if (Languaje.transform.GetChild(i).gameObject.name == "Ingles")
                {
                    LanguajeCurrent.transform.GetChild(0).gameObject.transform.SetParent(Languaje.transform);
                    Languaje.transform.GetChild(i).gameObject.transform.SetParent(LanguajeCurrent.transform);
                }
            }
            LanguajeCurrent.transform.GetChild(0).gameObject.GetComponentInChildren<Button>().interactable = false;
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                Languaje.transform.GetChild(i).gameObject.GetComponentInChildren<Button>().interactable = true;
            }
        }
        if (PlayerPrefs.GetInt("Idioma") == 2)
        {
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                if (Languaje.transform.GetChild(i).gameObject.name == "Frances")
                {
                    LanguajeCurrent.transform.GetChild(0).gameObject.transform.SetParent(Languaje.transform);
                    Languaje.transform.GetChild(i).gameObject.transform.SetParent(LanguajeCurrent.transform);
                }
            }
            LanguajeCurrent.transform.GetChild(0).gameObject.GetComponentInChildren<Button>().interactable = false;
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                Languaje.transform.GetChild(i).gameObject.GetComponentInChildren<Button>().interactable = true;
            }
        }
        if (PlayerPrefs.GetInt("Idioma") == 3)
        {
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                if (Languaje.transform.GetChild(i).gameObject.name == "Portugues")
                {
                    LanguajeCurrent.transform.GetChild(0).gameObject.transform.SetParent(Languaje.transform);
                    Languaje.transform.GetChild(i).gameObject.transform.SetParent(LanguajeCurrent.transform);
                }
            }
            LanguajeCurrent.transform.GetChild(0).gameObject.GetComponentInChildren<Button>().interactable = false;
            for (int i = 0; i < Languaje.transform.childCount; i++)
            {
                Languaje.transform.GetChild(i).gameObject.GetComponentInChildren<Button>().interactable = true;
            }
        }
    }
}