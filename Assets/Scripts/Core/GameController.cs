using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<GameController>();
                _instance.Init();
            }

            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public LevelController LevelController;

    public PlayerController Player;
    public PlayerUI PlayerUI;
    public EndGameUI EndGameUI;
    public SoundController SoundController;

    private Dictionary<string, List<Action>> Events;

    private bool Started = false;

    public void Awake()
    {
        if (!Started)
        {
            Init();
        }
    }

    public void Init()
    {
        if (Started) return;

        Events = new Dictionary<string, List<Action>>();
    }

    public void AddEventListener(string eventName, Action cb)
    {
        if (!Events.ContainsKey(eventName))
        {
            var list = new List<Action>();
            list.Add(cb);
            Events.Add(eventName, list);
        }
        else
        {
            Events[eventName].Add(cb);
        }
    }

    public void RemoveEventListener(string eventName, Action cb)
    {
        if (Events.ContainsKey(eventName))
        {
            Events[eventName].RemoveAll(evt => evt == cb);

            if (Events[eventName].Count == 0)
            {
                Events.Remove(eventName);
            }
        }
    }

    public void DispatchEvent(string eventName)
    {
        if (Events.TryGetValue(eventName, out var list))
        {
            list.ForEach(cb => cb.Invoke());
        }
    }
}
