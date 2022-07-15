using System;
using UnityEngine;

public static class IdiomaManager
{
    public static event Action OnIdiomaSelect;

    public static void IdiomaUpdated() => OnIdiomaSelect?.Invoke();

    public static int SelectedIdioma => PlayerPrefs.GetInt("Idioma");
}
