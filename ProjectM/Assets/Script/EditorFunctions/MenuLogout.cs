#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Firebase.Auth;

public class MenuLogout : MonoBehaviour
{
    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("Mis Funciones/ Logout Firebase")]
    static void LogOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        if(auth!=null)
        {
            Debug.Log("cerrando sesion");
            auth.SignOut();
            Application.Quit();
        }
        else
        {
            Debug.LogError("no hay una instancia de firebase");
        }
    }

    [MenuItem("Mis Funciones/ Idioma/ Español")]
    static void Español()
    {
        PlayerPrefs.SetInt("Idioma", 0);
    }
    [MenuItem("Mis Funciones/ Idioma/ Ingles")]
    static void Ingles()
    {
        PlayerPrefs.SetInt("Idioma", 1);
    }
    [MenuItem("Mis Funciones/ Idioma/ Frances")]
    static void Frances()
    {
        PlayerPrefs.SetInt("Idioma", 2);
    }
    [MenuItem("Mis Funciones/ Idioma/ Portuges")]
    static void Portuges()
    {
        PlayerPrefs.SetInt("Idioma", 3);
    }
    [MenuItem("Mis Funciones/ Delete PlayerPrefs")]
    static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif