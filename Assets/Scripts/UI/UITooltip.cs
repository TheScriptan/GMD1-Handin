using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITooltip : Singleton<UITooltip>
{
    private Text textComponent;
    private IEnumerator messageCoroutine;
    public bool isShowing;

    void Start()
    {
        textComponent = GetComponent<Text>();
        textComponent.text = "";
        isShowing = false;
        messageCoroutine = ShowMessageTimer();
    }
    

    public void ShowTooltip(string text)
    {
        if (isShowing)
        {
            StopCoroutine(messageCoroutine);
        }
        isShowing = true;
        textComponent.text = text;
        StartCoroutine(messageCoroutine);
    }

    public void HideTooltip()
    {
        isShowing = false;
        textComponent.text = "";
        StopCoroutine(messageCoroutine);
    }
    
    IEnumerator ShowMessageTimer()
    {
        float delay = 3f;
        float normalizedTime = 0;
        isShowing = true;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / delay;
            yield return null;
        }

        HideTooltip();
    }
}