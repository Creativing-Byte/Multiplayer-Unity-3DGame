using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LoginRegistre : MonoBehaviour
{
    public InputField[] LoginRegistreInput; //0 email, 1 password, 2 email, 3 username, 4 password, 5 re-password, 6 email recovery
    public Button[] ButtonLoginRegistre; // 0 login, 1 registre, 2 Recovery;
    public TextMeshProUGUI[] Errormsg; // 0 login, 1 registre, 2 recovery
    public GameObject[] Advertencia;// 0 login, 1 registre, 2 Recovery

    int seconds;
    float secondsF;
    bool msgactive;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public async void Login(FirebaseAuth auth) // 6.B.A) Funcion de inicio de session de usuario : Converge en AuthStateChanged();
    {
        if (emailverify(LoginRegistreInput[0].text, 0) && passverify(LoginRegistreInput[1].text, 0))
        {
            await auth.SignInWithEmailAndPasswordAsync(LoginRegistreInput[0].text, LoginRegistreInput[1].text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    msgerror(0,"Error de conexion, Intente nuevamente.");
                    ButtonLoginRegistre[0].interactable = true;
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    const string Msg = "Correo o contraseña incorrecta, por favor verifique";
                    msgerror(0, Msg);
                    ButtonLoginRegistre[0].interactable = true;
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        }
        else
        {
            ButtonLoginRegistre[0].interactable = true;
            return;
        }
    }
    public async void Registre(FirebaseAuth auth) // 6.B.B) Funcion de registro de usuario : Converge en AuthStateChanged();
    {
        if (emailverify(LoginRegistreInput[2].text, 1) && userverify(LoginRegistreInput[3].text, 1) && passverify(LoginRegistreInput[4].text, 1) && passverify(LoginRegistreInput[5].text, 1))
        {
            if (LoginRegistreInput[4].text == LoginRegistreInput[5].text)
            {
                var user = await auth.CreateUserWithEmailAndPasswordAsync(LoginRegistreInput[2].text, LoginRegistreInput[5].text).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                        msgerror(1,"Error de conexion, Intente nuevamente.");
                        ButtonLoginRegistre[1].interactable = true;
                        return null;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                        msgerror(1,"Correo actualmente en uso, por favor verifique");
                        ButtonLoginRegistre[1].interactable = true;
                        return null;
                    }

                    return task.Result;
                });

                if (user != null)
                {
                    Debug.LogFormat("Firebase user created successfully: {0} ({1})", user.DisplayName, user.UserId);
                    List<string> carta = Enumerable.Range(0, 8).Select(x => x.ToString()).ToList();
                    List<string> friends = new List<string>();
                    string deck1 = Guid.NewGuid().ToString();
                    GameObject Launcher = GameObject.FindGameObjectWithTag("Launcher");
                    var userDb = Launcher.GetComponent<UserDbInit>();
                    await userDb.writeNewUser(user.UserId, LoginRegistreInput[3].text, user.Email, 10, 50, 0, 0, 0, DateTime.UtcNow.ToString("MM'/'dd'/'yyyy' 'HH':'mm':'ss"), "Tutorial", deck1, deck1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), carta, friends, "None");
                    userDb.reloadDate();
                }
            }
            else
            {
                msgerror(1,"Las contrasenas no coinciden");
                ButtonLoginRegistre[0].interactable = true;
                ButtonLoginRegistre[1].interactable = true;
            }
        }
    }
    public void RecoveryPassword(FirebaseAuth auth)// 6.B.C) Funcion de registro de usuario : Finaliza el Flujo;
    {
        if (emailverify(LoginRegistreInput[6].text, 2))
        {
            auth.SendPasswordResetEmailAsync(LoginRegistreInput[6].text).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    msgerror(2, "Error en la red, intente nuevamente.");
                    return;
                }
                if (task.IsFaulted)
                {
                    msgerror(22, task.Exception.ToString());
                    return;
                }
                ButtonLoginRegistre[0].interactable = true;
                ButtonLoginRegistre[1].interactable = true;
                ButtonLoginRegistre[2].interactable = true;
                blankspace();
                msgerror(2,"Password reset email sent successfully.");
                Debug.Log("Password reset email sent successfully.");
            });
        }
    }
    public void blankspace()
    {
        for (int i = 0; i < LoginRegistreInput.Length; i++)
        {
            LoginRegistreInput[i].text = "";
        }
    }
    bool emailverify(string email, int pos)
    {
        string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (string.IsNullOrEmpty(email))
        {
            Debug.Log("false");
            msgerror(pos, "No puede dejar el espacio en blanco");
            ButtonLoginRegistre[0].interactable = true;
            ButtonLoginRegistre[1].interactable = true;
            ButtonLoginRegistre[2].interactable = true;
            return false;
        }
        else
        {
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    Debug.Log("false");
                    msgerror(pos, "Formato incorrecto");
                    ButtonLoginRegistre[0].interactable = true;
                    ButtonLoginRegistre[1].interactable = true;
                    ButtonLoginRegistre[2].interactable = true;
                    return false;
                }
            }
            else
            {
                Debug.Log("false");
                msgerror(pos, "Formato incorrecto");
                ButtonLoginRegistre[0].interactable = true;
                ButtonLoginRegistre[1].interactable = true;
                ButtonLoginRegistre[2].interactable = true;
                return false;
            }
        }
    }
    bool passverify(string pass, int pos)
    {
        if (pass != String.Empty)
        {
            if (pass.Length > 7)
            {
                bool contNum = false;
                bool contString = false;
                foreach (char letter in pass)
                {
                    if (Char.IsNumber(letter))
                    {
                        contNum = true;
                    }


                    if (Char.IsLetter(letter))
                    {
                        contString = true;
                    }

                }
                if (!contString)
                {
                    Debug.LogError("Debe contener letras");
                    msgerror(pos, "Debe contener al menos una letra");
                    ButtonLoginRegistre[0].interactable = true;
                    ButtonLoginRegistre[1].interactable = true;
                    ButtonLoginRegistre[2].interactable = true;
                    return false;
                }

                if (!contNum)
                {
                    Debug.LogError("Debe contener numeros");
                    msgerror(pos, "Debe contener al menos un numero");
                    ButtonLoginRegistre[0].interactable = true;
                    ButtonLoginRegistre[1].interactable = true;
                    ButtonLoginRegistre[2].interactable = true;
                    return false;
                }

                if (contString && contNum)
                {
                    if (Regex.Replace(pass, "[^0-9]", "") == string.Empty)
                    {
                        msgerror(pos, "Debe contener al menos un numero");
                        ButtonLoginRegistre[0].interactable = true;
                        ButtonLoginRegistre[1].interactable = true;
                        ButtonLoginRegistre[2].interactable = true;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    ButtonLoginRegistre[0].interactable = true;
                    return false;
                }
            }
            else
            {
                Debug.LogError("Minimo 8 Caracteres para password");
                msgerror(pos, "Minimo 8 Caracteres para password");
                ButtonLoginRegistre[0].interactable = true;
                ButtonLoginRegistre[1].interactable = true;
                ButtonLoginRegistre[2].interactable = true;
                return false;
            }
        }
        else
        {
            Debug.LogError("No puede dejar el espacio en blanco");
            msgerror(pos, "No puede dejar el espacio en blanco");
            ButtonLoginRegistre[0].interactable = true;
            ButtonLoginRegistre[1].interactable = true;
            ButtonLoginRegistre[2].interactable = true;
            return false;
        }
    }
    bool userverify(string user, int pos)
    {
        if (!string.IsNullOrEmpty(user))
        {
            if (user.Length > 5)
            {
                return true;
            }
            else
            {
                Debug.LogError("Minimo 6 Caracteres para username");
                msgerror(pos, "Minimo 6 Caracteres para username");
                ButtonLoginRegistre[0].interactable = true;
                ButtonLoginRegistre[1].interactable = true;
                ButtonLoginRegistre[2].interactable = true;
                return false;
            }
        }
        else
        {
            Debug.LogError("No puede dejar el espacio en blanco");
            msgerror(pos, "No puede dejar el espacio en blanco");
            ButtonLoginRegistre[0].interactable = true;
            ButtonLoginRegistre[1].interactable = true;
            ButtonLoginRegistre[2].interactable = true;
            return false;
        }
    }
    void msgerror(int pos, string msg)
    {
        if (msg == "Password reset email sent successfully.")
        {
            Errormsg[pos].text = msg;
            try
            {
                Errormsg[pos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Errormsg[pos].text;
            }
            catch (NullReferenceException)
            {
                Errormsg[pos].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Errormsg[pos].text;
            }
            
        }
        else
        {
            Errormsg[pos].text = msg.ToUpper();
            try
            {
                Errormsg[pos].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Errormsg[pos].text;
            }
            catch (NullReferenceException)
            {
                Errormsg[pos].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = Errormsg[pos].text;
            }
        }
        msgactive = true;
        Advertencia[pos].SetActive(true);
    }
    public void ChangeLanguajeExt(int languaje)
    {
        AuthInit Launcher = GameObject.FindGameObjectWithTag("Launcher").GetComponent<AuthInit>();
        Launcher.ChangeLanguaje(languaje);
    }
    void Update()
    {
        if (msgactive)
        {
            secondsF += Time.deltaTime;
            seconds = (int)secondsF;
            if (seconds >= 3)
            {
                seconds = 0;
                secondsF = 0;
                for (int i = 0; i < Errormsg.Length; i++)
                {
                    Errormsg[i].text = "";
                    try
                    {
                        Errormsg[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    }
                    catch (NullReferenceException)
                    {
                        Errormsg[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
                    }
                    Advertencia[i].SetActive(false);
                }
                msgactive = false;
            }
        }
    }
}
