using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManagerCard : MonoBehaviour
{
    public static GameManagerCard Instance;
    public Card cardPrefab;
    public Sprite cardBack;
    public Sprite[] cardFaces;

    private List<Card> cards;
    private List<int> cardIDs;
    public Card firstCard, secondCard;
    public Transform cardspanel;

    private int pairsMatched;
    private int totalPairs;
    private float timer;
    public TextMeshProUGUI timerText;

    private bool isGameOver;
    private bool isLevelFinished;

    public float maxTime = 60f;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        cards = new List<Card>();
        cardIDs = new List<int>();
        pairsMatched = 0;
        totalPairs = cardFaces.Length / 2;

        timer = maxTime;
        isGameOver = false;
        isLevelFinished = false;

        CreateCards();
        ShuffleCards();

    }
    void Update()
    {

        if (!isGameOver && !isLevelFinished)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                GameOver();
            }

        }
    }

    void CreateCards()
    {
        int numPairs = cardFaces.Length / 2;

        for (int i = 0; i < numPairs; i++)
        {
            // Hindi card
            Card cardA = Instantiate(cardPrefab, cardspanel);
            cardA.gameManager = this;
            cardA.cardID = i * 2;
            cardA.matchID = i;
            cards.Add(cardA);

            // English card
            Card cardB = Instantiate(cardPrefab, cardspanel);
            cardB.gameManager = this;
            cardB.cardID = i * 2 + 1;
            cardB.matchID = i;
            cards.Add(cardB);
        }

        ShuffleCards(); 
    }

    void ShuffleCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(i, cards.Count);

            // Swap cards visually in the hierarchy by changing their sibling index
            Transform temp = cards[i].transform;
            cards[i].transform.SetSiblingIndex(cards[randomIndex].transform.GetSiblingIndex());
            cards[randomIndex].transform.SetSiblingIndex(temp.GetSiblingIndex());

            // Also swap in the list
            var tempCard = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = tempCard;
        }
    }

    public void CardFlipped(Card flippedCard)
    {
        if (firstCard == null)
        {
            firstCard = flippedCard;
        }
        else if (secondCard == null)
        {
            secondCard = flippedCard;
            CheckMatch();
        }

    }
    void CheckMatch()
    {
        if (firstCard.matchID == secondCard.matchID)
        {
            pairsMatched++;
            if (pairsMatched == totalPairs)
            {
                LevelFinished();
            }
            firstCard = null;
            secondCard = null;
        }
        else
        {
            StartCoroutine(FlipBackCards());
        }
    }

    IEnumerator FlipBackCards()
    {
        yield return new WaitForSeconds(1f);
        firstCard.HideCard();
        secondCard.HideCard();
        firstCard = null;
        secondCard = null;
    }

    void GameOver()
    {
        isGameOver = true;
    }
    void LevelFinished()
    {
        isLevelFinished = true;
    }
    void UpdateTimerText()
    {
        timerText.text = "Timer: " + Mathf.Round(timer) + "s";
    }
}