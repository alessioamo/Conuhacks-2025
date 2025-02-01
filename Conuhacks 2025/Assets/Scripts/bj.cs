using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BlackjackGame : MonoBehaviour
{
    public Transform playerSpawnPoint; // Assign in Inspector
    public Transform dealerSpawnPoint; // Assign in Inspector

    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> playerHand = new List<GameObject>();
    private List<GameObject> dealerHand = new List<GameObject>();

    private int playerScore = 0;
    private int dealerScore = 0;
    private bool playerTurn = true;

    void Start()
    {
        LoadDeck();
        ShuffleDeck();
        StartGame();
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
        DealCard(playerHand, playerSpawnPoint, 0);
        DealCard(dealerHand, dealerSpawnPoint, 0);
        DealCard(playerHand, playerSpawnPoint, 1);
        DealCard(dealerHand, dealerSpawnPoint, 1, true); // Face down

        playerScore = CalculateHandValue(playerHand);
        dealerScore = CalculateHandValue(dealerHand);

        Debug.Log($"Player Score: {playerScore}, Dealer Score: {dealerScore}");
    }

    Sprite originalFaceDown;

    void DealCard(List<GameObject> hand, Transform spawnPoint, int index, bool faceDown = false)
    {
        if (deck.Count == 0) return;

        GameObject card = deck[0];
        deck.RemoveAt(0);

        Vector3 position = spawnPoint.position + new Vector3(index * 1.5f, 0, 0);
        GameObject cardInstance = Instantiate(card, position, Quaternion.identity);
        
        if (faceDown)
        {
            // cardInstance.transform.Rotate(0, 180, 0); // Simulate face-down
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
            string cardName = card.name;
            int cardValue = GetCardValue(cardName);
            value += cardValue;

            if (cardValue == 11) aceCount++; // Ace handling
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10;
            aceCount--;
        }

        return value;
    }

    int GetCardValue(string cardName)
    {
        if (cardName.Contains("Ace")) return 11;
        if (cardName.Contains("King") || cardName.Contains("Queen") || cardName.Contains("Jack")) return 10;

        int number;
        if (int.TryParse(cardName.Substring(0, 1), out number)) return number; // Extract number for 2-10 cards

        return 0;
    }

    public void PlayerHit()
    {
        if (!playerTurn) return;

        DealCard(playerHand, playerSpawnPoint, playerHand.Count);
        playerScore = CalculateHandValue(playerHand);

        Debug.Log($"Player Score: {playerScore}");
        
        if (playerScore > 21)
        {
            Debug.Log("Player Bust! Dealer Wins.");
            playerTurn = false;
        }
    }

    public void PlayerStand()
    {
        if (!playerTurn) return;
        playerTurn = false;

        RevealDealerCard();
        DealerTurn();
    }

    void RevealDealerCard()
    {
        if (dealerHand.Count > 1)
        {
            // dealerHand[1].transform.Rotate(0, -180, 0); // Flip face-down card
            dealerHand[1].gameObject.GetComponent<SpriteRenderer>().sprite = originalFaceDown;
        }
    }

    void DealerTurn()
    {
        while (dealerScore < 17)
        {
            DealCard(dealerHand, dealerSpawnPoint, dealerHand.Count);
            dealerScore = CalculateHandValue(dealerHand);
            Debug.Log($"Dealer Score: {dealerScore}");
        }

        CheckGameResult();
    }

    void CheckGameResult()
    {
        if (dealerScore > 21)
        {
            Debug.Log("Dealer Bust! Player Wins.");
        }
        else if (playerScore > dealerScore)
        {
            Debug.Log("Player Wins!");
        }
        else if (playerScore < dealerScore)
        {
            Debug.Log("Dealer Wins.");
        }
        else
        {
            Debug.Log("Push! It's a Tie.");
        }
    }
}
