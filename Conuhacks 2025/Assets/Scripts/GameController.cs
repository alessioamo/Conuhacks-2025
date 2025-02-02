using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public int playerBalance = 10; // Default starting money

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject horseExit;
    public GameObject horseHover;
    public void AddMoney(int amount)
    {
        playerBalance += amount;
        Debug.Log($"Money Added: ${amount}, New Balance: ${playerBalance}");

        if (playerBalance >= 100) {
            horseExit.SetActive(true);
            horseHover.SetActive(true);
        }
    }

    public bool DeductMoney(int amount)
    {
        if (playerBalance >= amount)
        {
            playerBalance -= amount;
            Debug.Log($"Money Deducted: ${amount}, New Balance: ${playerBalance}");
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DecreaseBet(int value) {
        betAmount -= value;
    }

    public void IncreaseBet(int value) {
        betAmount += value;
    }

    public void ResetBet() {
        betAmount = 0;
    }

    public int betAmount = 0;

    public GameObject gameDisplay;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI betText;
    public String[] titles;
    public String[] descriptions;
    public String[] bets;

    int currentGameIndex;
    public void DisplayGameInfo(int index) {
        currentGameIndex = index;
        gameDisplay.gameObject.SetActive(true);
        titleText.text = titles[index];
        descriptionText.text = descriptions[index];
        betText.text = bets[index];
    }

    public void HideGameInfo() {
        gameDisplay.gameObject.SetActive(false);
    }

    public void StartMinigame() {
        HideGameInfo();
        if (currentGameIndex != 4) {
            SceneManager.LoadScene(currentGameIndex+2);
        }
    }

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    public GameObject enemyToFight;
    
    public void LoadBossFight(int index) {
        switch (index) {
            case 0:
                enemyToFight = enemy1;
                break;
            case 1:
                enemyToFight = enemy2;
                break;
            case 2:
                enemyToFight = enemy3;
                break;
            case 3:
                enemyToFight = enemy4;
                break;
        }
        
        SceneManager.LoadScene(5);
    }

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public int currentPlayerHealth;
}
