using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardID; // used to load sprite
    public int matchID; // used for logic matching
    public GameManagerCard gameManager;
    private bool isFlipped;
    public Image cardImage;

    void Start()
    {
        isFlipped = false;
        cardImage.sprite = GameManagerCard.Instance.cardBack;
    }

    public void FlipCard()
    {
        if (!isFlipped && gameManager.firstCard != this && gameManager.secondCard == null)
        {
            StartCoroutine(FlipAnimation());
        }
    }

    //Flipping the card
    IEnumerator FlipAnimation()
    {
        float duration = 0.15f;
        float elapsed = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 scaleZero = new Vector3(0, originalScale.y, originalScale.z);

        // Shrink to middle
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, scaleZero, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = scaleZero;

        // Changing sprite to front
        isFlipped = true;
        cardImage.sprite = gameManager.cardFaces[cardID];

        // Expanding it back
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(scaleZero, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;

        gameManager.CardFlipped(this);
    }

    public void HideCard()
    {
        StartCoroutine(FlipBackAnimation());
    }

    IEnumerator FlipBackAnimation()
    {
        float duration = 0.15f;
        float elapsed = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 scaleZero = new Vector3(0, originalScale.y, originalScale.z);

        // Shrinking to middle
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, scaleZero, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = scaleZero;

        // Changing sprite to back
        isFlipped = false;
        cardImage.sprite = gameManager.cardBack;

        // Expanding back
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(scaleZero, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
