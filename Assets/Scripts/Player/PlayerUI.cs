using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("General")]
    public Canvas Canvas;

    public TMP_Text ColonyPopulationTxt;
    public TMP_Text ColonyFoodTxt;
    public TMP_Text ColonyWorkersTxt;
    public TMP_Text ColonyMoneyTxt;

    [Header("Shop")]
    public CanvasRenderer Shop;
    public List<ShopWork> ShopWorks = new List<ShopWork>();
    public GameObject ShopWorksTemplate;
    public ShopItemUI ShopRowTemplate;

    [Header("Colelctable")]
    public CollectableUI CollectableUI;

    [Header("Game")]
    public GameObject PreCallGroup;
    public Image[] RefuseImagesNoFill;
    public Image[] RefuseImages;

    public void Start()
    {
        GameController.Instance.AddEventListener("UpdateUiStats", UpdateUiStats);
        GameController.Instance.AddEventListener("UpdateUiCurrentAi", UpdateUiCurrentAi);
        GameController.Instance.AddEventListener("OnAiReachEntrance", UpdateUiCurrentAi);
        GameController.Instance.AddEventListener("OnLevelCurrentAiChange", UpdateUiCurrentAi);
        GameController.Instance.AddEventListener("OnToggleShop", ToggleShop);

        UpdateUiStats();
        UpdateUiCurrentAi();
        HideShop();
        CreateShopItems();
    }

    public void UpdateUiStats()
    {
        var level = GameController.Instance.LevelController;

        ColonyPopulationTxt.text = level.CurrentPopulation + " / " + level.MaxCurrentPopulation;
        ColonyFoodTxt.text = level.Food + " / " + level.MaxFood;
        ColonyWorkersTxt.text = level.Workers + " / " + level.MaxWorkers;
        ColonyMoneyTxt.text = level.Money.ToString() + "$";

        for (int i = 0; i < RefuseImages.Length; ++i)
        {
            RefuseImages[i].gameObject.SetActive(level.RefusedPlays >= i + 1);
        }

        for (int i = 0; i < RefuseImagesNoFill.Length; ++i)
        {
            RefuseImagesNoFill[i].gameObject.SetActive(level.RefusedPlays < i + 1);
        }
    }

    private void UpdateUiCurrentAi()
    {
        var ai = GameController.Instance.Player.CurrentAi;

        if (ai)
        {
            PreCallGroup.gameObject.SetActive(false);
        }
        else
        {
            PreCallGroup.gameObject.SetActive(true);
        }
    }

    public void DispatchEvent(string eventName)
    {
        GameController.Instance.DispatchEvent(eventName);
    }

    private void HideShop()
    {
        Shop.gameObject.SetActive(false);
    }

    public void ToggleShop()
    {
        Shop.gameObject.SetActive(!Shop.gameObject.activeSelf);
    }

    public bool HasWork(string task) => ShopWorks.Find(work => work.Task == task) != null;

    public void ShopAddPopulation()
    {
        string task = "POPULATION";

        if (HasWork(task)) return;

        //// CREATE SHOP WORK
    }

    private void CreateShopItems()
    {
        ShopRowTemplate.gameObject.SetActive(false);
    }
}
