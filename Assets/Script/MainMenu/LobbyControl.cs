using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LobbyControl : MonoBehaviour
{
    public GameObject PanelLoading;
    public TextMeshProUGUI TimeLoadingSeconds;
    float timer = 0;


    private void Update()
    {
        if (PanelLoading.activeSelf)
        {
            timer += Time.deltaTime;
            TimeSpan ts_ = TimeSpan.FromSeconds(timer);
            TimeLoadingSeconds.text = ts_.Minutes.ToString("00") + ":" + ts_.Seconds.ToString("00");
            try
            {
                TimeLoadingSeconds.gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = TimeLoadingSeconds.text;
            }
            catch (UnityException)
            {
            }
        }
    }

    public void ActivarPanelLoading()
    {
        timer = 0;
        //PanelLoading.SetActive(true);
        if (fadeCor == null)
            fadeCor = StartCoroutine(FadeLoadingPanel(1));
    }
    private Coroutine fadeCor;
    private IEnumerator FadeLoadingPanel(float targetAlpha)
    {
        PanelLoading.SetActive(true);
        CanvasGroup cg = PanelLoading.GetComponent<CanvasGroup>();

        while (cg.alpha != targetAlpha)
        {
            cg.alpha += Mathf.Sign(targetAlpha - cg.alpha) * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        cg.alpha = targetAlpha;
        PanelLoading.SetActive(cg.alpha != 0);
        fadeCor = null;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void DesactivarPanelLoading()
    {
        timer = 0;
        PanelLoading.SetActive(false);
    }

    public void CancelMatchMaking()
    {
        //PanelLoading.SetActive(false);
        if (fadeCor == null)
            fadeCor = StartCoroutine(FadeLoadingPanel(0));
        timer = 0;
        PhotonInit.PhotonInitInstance.switchToRoom("Lobby");
    }
}
