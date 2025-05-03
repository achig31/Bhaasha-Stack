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
    public GameObject gameOverpanel;

    public ExpandPanel detailsPanel;
    public matchesPanel matchesPanel;

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
            //Pause timer if details panel is open
            if (detailsPanel != null && detailsPanel.IsExpanded)
            {
                return; // Skip timer update
            }

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

            // Expand details panel on successful match
            if (detailsPanel != null)
            {
                detailsPanel.ExpandPanelExternally();
            }

            StartCoroutine(HandleMatchedPair(firstCard, secondCard));

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
        StartCoroutine(ShowGameOverpanelDelayed());

        IEnumerator ShowGameOverpanelDelayed()
        {
            yield return new WaitForSeconds(1.25f);
            cardspanel.gameObject.SetActive(false);
            foreach (var expand in FindObjectsOfType<ExpandPanel>())
            {
                expand.isInteractable = false;
            }
            gameOverpanel.SetActive(true);
        }

    }
    void LevelFinished()
    {
        isLevelFinished = true;
        gameOverpanel.SetActive(true);
        cardspanel.gameObject.SetActive(false);
    }
    void UpdateTimerText()
    {
        timerText.text = "TIME: " + Mathf.Round(timer) + "s";
    }
    private IEnumerator HandleMatchedPair(Card cardA, Card cardB)
    {
        // Wait for details panel to collapse
        while (detailsPanel != null && detailsPanel.IsExpanded)
        {
            yield return null;
        }

        if (matchesPanel != null)
        {
            Transform slot = matchesPanel.GetNextSlot();
            if (slot != null)
            {
                var animatorA = cardA.GetComponent<MatchedCardAnimator>();
                var animatorB = cardB.GetComponent<MatchedCardAnimator>();

                if (animatorA != null && animatorB != null)
                {
                    // Stack cardA on top of cardB with slight Y offset
                    Vector3 offsetA = new Vector3(0f, 20f, 0f);
                    Vector3 offsetB = new Vector3(0f, -20f, 0f);

                    // Animate both cards into the slot
                    yield return StartCoroutine(animatorA.AnimateToSlot(slot, offsetA));
                    yield return StartCoroutine(animatorB.AnimateToSlot(slot, offsetB));
                }
            }
        }

        if (pairsMatched == totalPairs)
        {
            LevelFinished();
        }
    }

}