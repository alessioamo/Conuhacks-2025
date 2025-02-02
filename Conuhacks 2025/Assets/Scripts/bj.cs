using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class BlackjackGame : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform dealerSpawnPoint;

    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> playerHand = new List<GameObject>();
    private List<GameObject> dealerHand = new List<GameObject>();

    private int playerScore = 0;
    private int dealerScore = 0;
    private bool playerTurn = true;
    private int currentBet = 0;
    private bool gameInProgress = false;

     // Add UI text references
    public TextMeshProUGUI balanceText;  // Reference to the Text object that shows the player's balance
    public TextMeshProUGUI scoreText;    // Reference to the Text object that shows the player's score
    public TextMeshProUGUI betText;
    public TextMeshProUGUI dealerScoreText;

    public TextMeshProUGUI resultText;

    void Start()
    {
        LoadDeck();
        ShuffleDeck();
        UpdateUI();
    }

    void Update()
    {
        if (!gameInProgress && Input.GetKeyDown(KeyCode.B))
        {
            if (GameController.Instance.betAmount <= 0) {
                Debug.Log("Not enough bet");
            }
            else {
                PlaceBet(GameController.Instance.betAmount);
            }
        }
        else if (gameInProgress)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                PlayerTurn("hit");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                PlayerTurn("stand");
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && !gameInProgress) {
            SceneManager.LoadScene(1);
        }
    }

    public void PlaceBet(int amount)
    {
        if (GameController.Instance.DeductMoney(amount))
        {
            currentBet = amount;
            Debug.Log($"Bet Placed: ${currentBet}, Balance Left: ${GameController.Instance.playerBalance}");
            StartGame();
        }
        else
        {
            Debug.Log("Not enough money to place bet!");
        }
    }

    void LoadDeck()
    {
        GameObject[] loadedCards = Resources.LoadAll<GameObject>("Cards");
        deck.AddRange(loadedCards);
    }

    void ShuffleDeck()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (deck[i], deck[randomIndex]) = (deck[randomIndex], deck[i]); // Swap elements
        }
    }

    void StartGame()
    {
        resultText.gameObject.SetActive(false);

        ClearTable();
        gameInProgress = true;
        playerHand.Clear();
        dealerHand.Clear();
        playerScore = 0;
        dealerScore = 0;
        playerTurn = true;

        DealCard(playerHand, playerSpawnPoint, 0);
        DealCard(dealerHand, dealerSpawnPoint, 0);
        DealCard(playerHand, playerSpawnPoint, 1);
        DealCard(dealerHand, dealerSpawnPoint, 1, true);

        playerScore = CalculateHandValue(playerHand);
        dealerScore = CalculateHandValue(dealerHand);

        Debug.Log($"Player Score: {playerScore}, Dealer Score: {dealerScore}");

        UpdateUI();

        if (playerScore == 21)
        {
            EndRound(true);
        }
    }

    Sprite originalFaceDown;
    void DealCard(List<GameObject> hand, Transform spawnPoint, int index, bool faceDown = false)
    {
        if (deck.Count == 0) return;

        GameObject card = deck[0];
        deck.RemoveAt(0);

        Vector3 position = spawnPoint.position + new Vector3(index * 2.05f, 0, 0);
        GameObject cardInstance = Instantiate(card, position, Quaternion.identity);
        
        if (faceDown)
        {
            //cardInstance.transform.Rotate(0, 180, 0);
            originalFaceDown = cardInstance.gameObject.GetComponent<SpriteRenderer>().sprite;
            cardInstance.gameObject.GetComponent<SpriteRenderer>().sprite = cardInstance.gameObject.GetComponent<Card>().imageBack;
        }

        hand.Add(cardInstance);
    }

    int CalculateHandValue(List<GameObject> hand)
    {
        int value = 0;
        int aceCount = 0;

        foreach (var card in hand)
        {
            Card cardComponent = card.GetComponent<Card>();
            if (cardComponent != null)
            {
                int cardValue = cardComponent.value;
                value += cardValue;
                if (cardValue == 11) aceCount++;
            }
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10;
            aceCount--;
        }

        return value;
    }

    public void PlayerTurn(string action)
    {
        if (!playerTurn) return;

        if (action == "hit")
        {
            DealCard(playerHand, playerSpawnPoint, playerHand.Count);
            playerScore = CalculateHandValue(playerHand);
            Debug.Log($"Player Score: {playerScore}");
            UpdateUI();

            if (playerScore > 21)
            {
                Debug.Log("Player Bust! Dealer Wins.");
                ShowResultText("dealer");
                EndRound(false);
            }
        }
        else if (action == "stand")
        {
            playerTurn = false;
            RevealDealerCard();
            DealerTurn();
        }
    }

    void RevealDealerCard()
    {
        if (dealerHand.Count > 1)
        {
            //dealerHand[1].transform.Rotate(0, -180, 0);
            dealerHand[1].gameObject.GetComponent<SpriteRenderer>().sprite = originalFaceDown;
            UpdateUI();
        }
    }

    void DealerTurn()
{
    StartCoroutine(DealerPlayCoroutine());
}

IEnumerator DealerPlayCoroutine()
{
    while (dealerScore < 17)
    {
        yield return new WaitForSeconds(1.5f); // Add delay for realism

        DealCard(dealerHand, dealerSpawnPoint, dealerHand.Count);
        dealerScore = CalculateHandValue(dealerHand);
        Debug.Log($"Dealer Score: {dealerScore}");
        UpdateUI();
    }

    CheckGameResult();
}

    void CheckGameResult()
    {
        if (dealerScore > 21 || playerScore > dealerScore)
        {
            Debug.Log("Player Wins!");
            EndRound(true);
        }
        else if (playerScore < dealerScore)
        {
            Debug.Log("Dealer Wins.");
            EndRound(false);
        }
        else
        {
            Debug.Log("Push! It's a Tie.");
            ShowResultText("tie");
            GameController.Instance.AddMoney(currentBet);
            currentBet = 0;
            gameInProgress = false;
        }
    }

    int playerWinCounter = 0;
    void EndRound(bool playerWins)
    {
        if (playerWins)
        {
            playerWinCounter++;
            int payout = (playerScore == 21 && playerHand.Count == 2) ? Mathf.RoundToInt(currentBet * 2.5f) : currentBet * 2;
            GameController.Instance.AddMoney(payout);
            Debug.Log($"Player Wins! New Balance: ${GameController.Instance.playerBalance}");
            ShowResultText("player");
            if (playerWinCounter >= 5) {
                playerWinCounter = 0;
                GameController.Instance.LoadBossFight(3);
            }
        }
        else
        {
            Debug.Log($"Dealer Wins. Remaining Balance: ${GameController.Instance.playerBalance}");
            ShowResultText("dealer");
        }

        currentBet = 0;
        gameInProgress = false;
        UpdateUI();
    }
    void ClearTable()
{
    foreach (var card in playerHand)
    {
        Destroy(card);
    }
    foreach (var card in dealerHand)
    {
        Destroy(card);
    }
}
 public void UpdateUI()
    {
        if (balanceText != null)
        {
            balanceText.text = $"Balance: ${GameController.Instance.playerBalance}";
        }

        if (scoreText != null)
        {
            scoreText.text = $"Player Score: {playerScore}";
        }
        if (betText != null) {
            betText.text = $"Bet Amount: {GameController.Instance.betAmount}";
        }
        if (dealerScoreText != null) {
            if (playerTurn) {
                dealerScoreText.text = $"Dealer Score: ?";
            }
            else {
                dealerScoreText.text = $"Dealer Score: {dealerScore}";
            }
        }
    }

    void ShowResultText(string name) {
        resultText.gameObject.SetActive(true);
        if (name == "player") {
            resultText.text = "You Win!";
        }
        else if (name == "dealer") {
            resultText.text = "You Lost!";
        }
        else if (name == "tie") {
            resultText.text = "It's a Tie!";
        }
    }
}


