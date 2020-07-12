using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMisc : MonoBehaviour
{
    public static UIMisc instance { get; private set; }
    public TextMeshProUGUI savingText;
    public TextMeshProUGUI savePromptText;
    public TextMeshProUGUI swordText;
    public TextMeshProUGUI zapText;
    public TextMeshProUGUI hoverText;

    float startTime = 3f;
    float timer;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, startTime);
        if (timer <= 0)
        {
            savingText.gameObject.SetActive(false);
            swordText.gameObject.SetActive(false);
            zapText.gameObject.SetActive(false);
            hoverText.gameObject.SetActive(false);
            savePromptText.gameObject.SetActive(false);
        }
    }

    public void ShowSave()
    {
        savingText.gameObject.SetActive(true);
        savePromptText.gameObject.SetActive(false);
        timer = startTime;
    }

    public void ExplainSword()
    {
        swordText.gameObject.SetActive(true);
        timer = startTime;
    }

    public void ExplainZap()
    {
        zapText.gameObject.SetActive(true);
        timer = startTime;
    }
    public void ExplainHover()
    {
        hoverText.gameObject.SetActive(true);
        timer = startTime;
    }

    public void ExplainSave()
    {
        if (!savingText.isActiveAndEnabled)
        {
            savePromptText.gameObject.SetActive(true);
            timer = startTime;
        }
    }
}
