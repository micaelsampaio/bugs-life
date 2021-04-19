using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public CanvasGroup CanvasGroup;

    public TMP_Text ColonyPopulationTxt;
    public TMP_Text ColonyFoodTxt;
    public TMP_Text ColonyWorkersTxt;
    public TMP_Text ColonyMoneyTxt;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        GameController.Instance.PlayerUI.gameObject.SetActive(false);
        GameController.Instance.SoundController.PlaySound("EndGame");
        gameObject.SetActive(true);
        StartCoroutine(FadeInRoutine(CanvasGroup, 1));
        UpdateEndStats();
    }

    public void UpdateEndStats()
    {
        var level = GameController.Instance.LevelController;

        ColonyPopulationTxt.text = level.CurrentPopulation + " / " + level.MaxCurrentPopulation;
        ColonyFoodTxt.text = level.Food + " / " + level.MaxFood;
        ColonyWorkersTxt.text = level.Workers + " / " + level.MaxWorkers;
        ColonyMoneyTxt.text = level.Money.ToString() + "$";
    }
    public IEnumerator FadeInRoutine(CanvasGroup FadeObject, float time)
    {
        float alpha = 0;
        FadeObject.gameObject.SetActive(true);
        while (alpha < 1)
        {
            Debug.Log("Fade In " + alpha);

            alpha += Time.deltaTime * time;
            FadeObject.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
        FadeObject.alpha = 1f;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
