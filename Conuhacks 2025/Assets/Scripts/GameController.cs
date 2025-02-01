using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public int playerBalance = 1000; // Default starting money

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

    public void AddMoney(int amount)
    {
        playerBalance += amount;
        Debug.Log($"Money Added: ${amount}, New Balance: ${playerBalance}");
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
}
