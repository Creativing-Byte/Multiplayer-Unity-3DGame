using Firebase;
using Firebase.Auth;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    protected FirebaseAuth auth;
    protected FirebaseUser user;
    public FirebaseUser User => user;

    public void LogOut()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        if (auth != null)
        {
            Debug.Log("cerrando sesion");
            auth.SignOut();
        }
        else
        {
            Debug.LogError("no hay una instancia de firebase");
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

}
