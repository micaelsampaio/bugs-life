using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectable_Controller", menuName = "ANT/Collectable/New Controller")]
public class CollectableController : ScriptableObject
{
    public List<Collectable> Collectables;

    public Collectable GetRandomCollectable()
    {
        var collectable = Collectables[Random.Range(0, Collectables.Count)];
        collectable.CurrentStats = collectable.Stats[Random.Range(0, collectable.Stats.Length)];

        return collectable;
    }

    public (bool, string) AcceptTrade(Collectable collectable, int amount = -1, bool updateData = false)
    {
        var level = GameController.Instance.LevelController;
        var player = GameController.Instance.Player;
        var stats = collectable.CurrentStats;

        var nextPopulation = level.CurrentPopulation + stats.Population + level.CurrentAi.Population;
        var nextWorkers = level.Workers + stats.Workers;
        var nextFood = level.Food + stats.Food;
        var nextMoney = level.Money + stats.Money - (amount > 0 ? amount : stats.Price);

        string errorDescription = "";
        bool hasError = false;

        if (nextPopulation >= level.MaxCurrentPopulation)
        {
            hasError = true;
            errorDescription += "\n Insuficient Population Size";
        }

        if (nextFood >= level.MaxFood)
        {
            hasError = true;
            errorDescription += "\n Insuficient Food Size";
        }

        if (nextWorkers >= level.MaxWorkers)
        {
            hasError = true;
            errorDescription += "\n Insuficient Workers Size";
        }

        if (nextMoney < 0)
        {
            hasError = true;
            errorDescription += "\n Insuficient Money";
        }

        if (!hasError && updateData)
        {
            // SEND EVENT FOR EFFECTS
            level.CurrentPopulation = nextPopulation;
            level.Food = nextFood;
            level.Workers = nextWorkers;
            level.Money = nextMoney;

            GameController.Instance.DispatchEvent("UpdateUiStats");
        }
        return (!hasError, errorDescription.Length == 0 ? "Unknown Error" : errorDescription);
    }
}