using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectableUI : MonoBehaviour
{
    [Header("Ant Info")]
    public TMP_Text AiName;
    public TMP_Text AiPopulation;
    public Image AiIcon;

    [Header("Offer Info")]

    public TMP_Text Description;
    public Image Icon;

    public TMP_Text PopulationTxt;
    public TMP_Text FoodTxt;
    public TMP_Text WorkersTxt;
    public TMP_Text MoneyTxt;
    public TMP_Text PriceTxt;
    public TMP_Text ErrorTxt;

    public GameObject OffersMoney;
    public GameObject OffersNoMoney;

    public Button[] OfferButtons;
    public Button OfferNoMoneyButtons;

    private Collectable Collectable;

    private void Start()
    {
        Hide();

        GameController.Instance.AddEventListener("OnUpdateStats", UpdateView);
        GameController.Instance.AddEventListener("OnAiReachEntrance", OnAiReachEntrance);
        GameController.Instance.AddEventListener("OnAiAccepted", Hide);
        GameController.Instance.AddEventListener("OnAiRefused", Hide);
    }

    private void OnAiReachEntrance()
    {
        if (GameController.Instance.LevelController.CurrentAi != null)
        {
            Show(GameController.Instance.LevelController.CurrentAi.Collectable);
        }
    }

    public void Show(Collectable collectable)
    {
        Collectable = collectable;

        if (Collectable != null)
        {
            gameObject.SetActive(true);
            UpdateView();
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateView()
    {
        if (!gameObject.activeSelf) return;

        var level = GameController.Instance.LevelController;
        var ai = level.CurrentAi;

        AiName.text = ai.FullName;
        AiPopulation.text = ai.Family;

        Description.text = Collectable.Description;
        Icon.sprite = Collectable.Icon;
        FoodTxt.text = ParseValues(level.Food, level.Food + Collectable.CurrentStats.Food);
        PopulationTxt.text = ParseValues(level.CurrentPopulation, level.CurrentPopulation + Collectable.CurrentStats.Population + level.CurrentAi.Population);
        WorkersTxt.text = ParseValues(level.Workers, level.Workers + Collectable.CurrentStats.Workers);
        MoneyTxt.text = ParseValues(level.Money, level.Money + Collectable.CurrentStats.Money);
        PriceTxt.text = Collectable.CurrentStats.Price.ToString() + "$";

        if (Collectable.CurrentStats.Price > 0)
        {
            PriceTxt.gameObject.SetActive(false);
            OffersMoney.SetActive(true);
            OffersNoMoney.SetActive(false);
        }
        else
        {
            PriceTxt.gameObject.SetActive(false);
            OffersMoney.SetActive(false);
            OffersNoMoney.SetActive(true);
        }

        var price = Collectable.CurrentStats.Price;
        var offerOffset = price > 1000 ? 1000 : price > 100 ? 100 : 10;
        int[] offers = { price - offerOffset, price, price + offerOffset };

        for (var i = 0; i < OfferButtons.Length; ++i)
        {
            int offerIndex = i;
            OfferButtons[offerIndex].GetComponentInChildren<TMP_Text>().text = offers[offerIndex].ToString();
            OfferButtons[offerIndex].onClick.RemoveAllListeners();
            OfferButtons[offerIndex].onClick.AddListener(() =>
            {
                if (!GameController.Instance.LevelController.AcceptTrade(offers[offerIndex]))
                {
                    ErrorTxt.text = GameController.Instance.LevelController.ErrorDescription;
                }
                Debug.Log("Offer " + offers[offerIndex]);
            });
        }

        OfferNoMoneyButtons.onClick.RemoveAllListeners();
        OfferNoMoneyButtons.onClick.AddListener(() =>
        {
            GameController.Instance.LevelController.AcceptTrade();
        });

        ErrorTxt.text = "";
    }

    private string ParseValues(int oldValue, int newValue)
    {
        return oldValue + (newValue > oldValue ? " -> " + newValue : "");
    }
}