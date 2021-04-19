using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AI CurrentAi;
    public Animator Animator;

    private void Start()
    {
        GameController.Instance.AddEventListener("OnAiReachEntrance", OnAiReachEntrance);
        GameController.Instance.AddEventListener("OnPlayerCallNextAi", OnPlayerCallNextAi);
        // GameController.Instance.AddEventListener("OnPlayerAcceptAi", OnPlayerAcceptAi);
        GameController.Instance.AddEventListener("OnLevelCurrentAiChange", OnLevelCurrentAiChange);
        GameController.Instance.AddEventListener("OnAiAccepted", () => Animator.SetTrigger("PointRight"));
        GameController.Instance.AddEventListener("OnAiRefused", () => Animator.SetTrigger("PointLeft"));

    }

    private void OnLevelCurrentAiChange()
    {
        CurrentAi = GameController.Instance.LevelController.CurrentAi;
    }

    private void OnPlayerCallNextAi()
    {
        if (CurrentAi != null)
        {
            //GameController.Instance.DispatchEvent("CreateAi");
            return;
        }
        if (GameController.Instance.LevelController.CurrentPopulation >= GameController.Instance.LevelController.MaxCurrentPopulation) return;

        Animator.SetTrigger("Call");
        GameController.Instance.DispatchEvent("OnLevelCallNextAi");

        Debug.Log("OnPlayerCallNextAi");
        //if (CurrentAi != null) return;
    }

    private void OnAiReachEntrance()
    {
        CurrentAi = GameController.Instance.LevelController.CurrentAi;

        Debug.Log("OnAiReachEntrance");

        //throw new NotImplementedException();
    }

    //public delegate void LoadLevelRequestHandler(int pLevelToLoad);

    //public static event LoadLevelRequestHandler onLoadLevelRequest;

}
