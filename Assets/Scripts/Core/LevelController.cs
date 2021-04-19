using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Ai")]
    public AI AiBase;
    public Queue<AI> AiInstances;
    public List<AI> CurrentAiInstances;
    public AI CurrentAi;
    public AIInfo AiCustomization;

    [Header("Colony")]
    public int Money;
    public int CurrentPopulation;
    public int MaxCurrentPopulation;
    public int Food;
    public int MaxFood;
    public int Workers;
    public int MaxWorkers;

    [Header("Others")]
    public int ReachPosition;
    public Transform[] Positions;
    public Transform AcceptPosition;
    public Transform RefusePosition;
    public Transform EntrancePosition;
    public int RefusedPlays = 0;

    [Header("Shopping")]
    public List<ShopItem> ShopItems;

    [Header("Collectables")]
    public CollectableController Collectables;

    public string ErrorDescription;

    // Start is called before the first frame update
    void Start()
    {
        AiInstances = new Queue<AI>();
        CurrentAiInstances = new List<AI>();
        SpawnAi();
        CreateAi();
        UpdateAiPositions();
        CreateShopItems();

        GameController.Instance.AddEventListener("OnPlayerRefuseAi", OnRefuseAi);
        GameController.Instance.AddEventListener("OnLevelCallNextAi", OnLevelCallNextAi);
        GameController.Instance.AddEventListener("CreateAi", () =>
        {
            AiCustomization.Customize(CurrentAi);
            GameController.Instance.DispatchEvent("OnLevelCurrentAiChange");
        });
        GameController.Instance.AddEventListener("UpdateUiStats", CheckEndGame);
        //GameController.Instance.AddEventListener("OnAiReachNextPosition", OnAiReachNextPosition);
    }

    private void CheckEndGame()
    {
        if (RefusedPlays >= 5)
        {
            GameController.Instance.EndGameUI.Show();
        }
    }

    private void SpawnAi(int size = 20)
    {
        AiBase.gameObject.SetActive(false);

        for (var i = 0; i < size; ++i)
        {
            var clone = Instantiate(AiBase);
            AiInstances.Enqueue(clone);
        }
    }

    private void CreateAi()
    {
        for (var i = 0; i < 5; ++i)
        {
            CurrentAiInstances.Add(AiCustomization.Customize(GetAiInstance()));
        }
    }

    private AI GetAiInstance()
    {
        var ai = AiInstances.Dequeue();
        var startPos = AiBase.transform.position;
        ai.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        ai.gameObject.SetActive(true);
        ai.transform.position = new Vector3(
            UnityEngine.Random.Range(startPos.x - 0.5f, startPos.x + 0.5f),
            startPos.y,
            Positions[CurrentAiInstances.Count].position.z + 3f);

        return ai;
    }

    private void UpdateAiPositions()
    {
        float delay = .0f;
        for (var i = 0; i < CurrentAiInstances.Count; ++i)
        {
            var ai = CurrentAiInstances[i];
            delay += UnityEngine.Random.Range(0, 1f);
            // ai.transform.position = new Vector3(ai.transform.position.x, ai.transform.position.y, Positions[i].position.z);
            ai.GoToNextPosition(
                new Vector3(ai.transform.position.x,
                ai.transform.position.y,
                Positions[i].position.z),
                delay
            );
        }

        if (CurrentAi != null)
        {
            //CurrentAi.transform.position = new Vector3(EntrancePosition.position.x, CurrentAi.transform.position.y, EntrancePosition.position.z);
            //new Vector3(ai.transform.position.x, ai.transform.position.y, Positions[i].position.z)
            CurrentAi.GoToEntrance(new Vector3(EntrancePosition.position.x, CurrentAi.transform.position.y, EntrancePosition.position.z));
        }
    }

    private void OnDrawGizmos()
    {
        if (Positions != null && Positions.Length > 0)
        {
            Gizmos.color = Color.blue;
            foreach (var pos in Positions)
            {
                if (pos != null)
                {
                    Gizmos.DrawWireSphere(pos.position, 0.5f);
                }
            }
        }

        if (EntrancePosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(EntrancePosition.position, 1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(EntrancePosition.position, 2f);
        }

        if (AcceptPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(AcceptPosition.position, 1f);
        }
        if (RefusePosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(RefusePosition.position, 1f);
        }
    }

    private void OnDisableAi(AI ai)
    {
        ai.gameObject.SetActive(false);
        AiInstances.Enqueue(ai);
    }

    public bool AcceptTrade(int money = -1)
    {
        var (done, errorString) = Collectables.AcceptTrade(CurrentAi.Collectable, money, true);

        ErrorDescription = errorString;

        if (done)
        {
            GameController.Instance.SoundController.PlaySound("Click");
            RefusedPlays = 0;
            CurrentAiInstances.Remove(CurrentAi);
            CurrentAi.OfferAccepted(AcceptPosition.position, OnDisableAi);
            CurrentAi = null;
            GameController.Instance.DispatchEvent("OnLevelCurrentAiChange");
            GameController.Instance.DispatchEvent("OnAiAccepted");
        }
        else
        {
            GameController.Instance.SoundController.PlaySound("ClickError");
            Debug.Log("Error " + errorString);
        }
        return done;
    }

    private void OnRefuseAi()
    {
        GameController.Instance.SoundController.PlaySound("RefuseAi");
        ++RefusedPlays;
        CurrentAiInstances.Remove(CurrentAi);
        CurrentAi.OfferRefused(RefusePosition.position, OnDisableAi);
        CurrentAi = null;
        GameController.Instance.DispatchEvent("OnLevelCurrentAiChange");
        GameController.Instance.DispatchEvent("OnAiRefused");
        GameController.Instance.DispatchEvent("UpdateUiStats");
    }


    private void OnLevelCallNextAi()
    {
        if (CurrentAi != null) return;

        GameController.Instance.SoundController.PlaySound("CallAi");
        ErrorDescription = "";
        ReachPosition = 0;
        CurrentAi = CurrentAiInstances[0];
        CurrentAi.Collectable = Collectables.GetRandomCollectable();
        CurrentAiInstances.Remove(CurrentAi);
        CurrentAiInstances.Add(AiCustomization.Customize(GetAiInstance()));
        UpdateAiPositions();
        GameController.Instance.DispatchEvent("OnLevelCurrentAiChange");
        // GameController.Instance.DispatchEvent("OnAiReachEntrance");
    }

    private void OnShop(ShopItem item)
    {

        var upgrade = item.Upgrades[item.Level];

        if (upgrade.Price > Money)
        {
            GameController.Instance.SoundController.PlaySound("ClickError");
            return;
        }

        GameController.Instance.SoundController.PlaySound("ShopClick");

        foreach (var it in upgrade.Items)
        {
            switch (it.Type)
            {
                case ShopItemType.POPULATION:
                    MaxCurrentPopulation += it.Amount;
                    break;
                case ShopItemType.MAX_FOOD:
                    MaxFood += it.Amount;
                    break;
                case ShopItemType.WORKERS:
                    MaxWorkers += it.Amount;
                    break;
                case ShopItemType.BADGES:
                default:
                    Debug.Log("Not Implemented " + it.Type);
                    break;
            }

            GameController.Instance.DispatchEvent("UpdateUiStats");
        }

        ++item.Level;

        Money -= upgrade.Price;
    }

    private void CreateShopItems()
    {
        var template = GameController.Instance.PlayerUI.ShopRowTemplate;
        template.gameObject.SetActive(false);
        foreach (var item in ShopItems)
        {
            var clone = Instantiate(template);
            clone.transform.SetParent(template.transform.parent);
            clone.transform.localScale = Vector3.one;
            clone.gameObject.SetActive(true);

            item.Init();
            item.UI = clone;
            item.UI.SetItem(item, OnShop);
        }
    }


    //private void OnAiReachNextPosition()
    //{
    //    ReachPosition++;

    //    if (ReachPosition >= 5)
    //    {
    //        Debug.Log("OnAiReachEntrance");
    //        //GameController.Instance.DispatchEvent("OnAiReachEntrance");
    //    }
    //}
}
