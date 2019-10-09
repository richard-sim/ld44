using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour {
    public GameLevelManager LevelManager;
    public PlayerController Player;
    
    public Button TradeBloodButton;
    public Button LeaveStoreButton;

    public TMP_Text CurrentLevelText;
    public TMP_Text CurrentScoreText;
    public TMP_Text PotionTimerText;

    private IEnumerator WaitUntilPlayResumes() {
        while ((LevelManager.State == GameLevelManager.GameState.InStore) || (LevelManager.State == GameLevelManager.GameState.Attacking)) {
//            Debug.Log(LevelManager.State.ToString());
            yield return null;
        }

        Debug.Log("Play has resumed.");
    }

    public void OnTradeBloodClicked() {
        Player.ApplyPotion();
        
        LevelManager.ChangeState(GameLevelManager.GameState.Playing);
        StartCoroutine(WaitUntilPlayResumes());
    }

    public void OnExitStoreClicked() {
        LevelManager.ChangeState(GameLevelManager.GameState.Playing);
        StartCoroutine(WaitUntilPlayResumes());
    }
    
    // Update is called once per frame
    void Update() {
        CurrentLevelText.text = $"Level {GameCoordinator.Instance.Level + 1}";
        CurrentScoreText.text = $"Health: {GameCoordinator.Instance.Health}%";

        int timeRemaining = Player.PotionTime - Player.PotionTimeElapsed;
        int potionMins = timeRemaining / 60;
        int potionSecs = timeRemaining % 60;
        PotionTimerText.text = $"{potionMins}:{potionSecs:D2}";

        TradeBloodButton.interactable = (GameCoordinator.Instance.Health > Player.PotionCost);
    }
}
