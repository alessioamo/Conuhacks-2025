using UnityEngine;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour
{
    public Transform spawnPoint; // Assign in Inspector
    private List<GameObject> deck = new List<GameObject>();

    void Start()
    {
        LoadDeck();
        ShuffleDeck();
        DealCards();
    }

    void LoadDeck()
    {
        GameObject[] loadedCards = Resources.LoadAll<GameObject>("Cards"); // Load all prefabs in Resources/Cards
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

    void DealCards()
    {
        float xOffset = 1.5f; // Space between cards
        for (int i = 0; i < deck.Count; i++)
        {
            Vector3 position = spawnPoint.position + new Vector3(i * xOffset, 0, 0); // Spread cards
            Instantiate(deck[i], position, Quaternion.identity);
        }
    }
}
