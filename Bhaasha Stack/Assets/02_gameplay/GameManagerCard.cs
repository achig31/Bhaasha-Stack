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
        for (int i = 0;  i < cardFaces.Length / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        foreach (int id in cardIDs)
        {
            Card newCard = Instantiate(cardPrefab, cardspanel);
            newCard.gameManager = this;
            newCard.cardID = id;
            cards.Add(newCard);
        }
    }
    void ShuffleCards()
    {
        for (int i = 0; i < cardIDs.Count; i++)
        {
            int randomIndex = Random.Range(i, cardIDs.Count);
            int temp = cardIDs[i];
            cardIDs[i] = cardIDs[randomIndex];
            cardIDs[randomIndex] = temp;
        }
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].cardID = cardIDs[i];
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
        if (firstCard.cardID == secondCard.cardID)
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