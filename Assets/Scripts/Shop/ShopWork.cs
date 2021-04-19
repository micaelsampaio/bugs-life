using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWork
{
    // UI
    public string Task;
    public GameObject Container;
    public TMP_Text Text;
    public Image Progress;

    // Logic
    private int Time;
    private int CurrentTime;
    private Action OnFinish;

    public void Start(string task, string text, int time, Action cb)
    {
        Task = task;
        Text.text = text;
        Time = time;
        CurrentTime = time;
        OnFinish = cb;

        UpdateUI();
    }

    public void Update()
    {
        if (--CurrentTime > 0)
        {
            UpdateUI();
        }
        else
        {
            OnFinish.Invoke();
        }
    }

    private void UpdateUI()
    {
        Progress.fillAmount = CurrentTime / Time;
    }
}
