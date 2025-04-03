using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  // If you're using regular Unity Text, otherwise use TMPro
using System.Collections;
using TMPro;

public class ExpandPanel : MonoBehaviour
{
    public Vector2 expandedSize = new Vector2(270, 745);
    public Vector2 collapsedSize = new Vector2(100, 745);
    public float animationDuration = 0.3f;

    private bool isExpanded = false;
    private RectTransform rectTransform;
    private Coroutine currentCoroutine;

    // Reference to the Text component
    public TMP_Text panelText;  // Use Text for Unity's standard UI Text or TMP_Text for TextMeshPro

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = collapsedSize; // Start collapsed

        // Ensure text is hidden when the panel is collapsed initially
        if (panelText != null)
            panelText.gameObject.SetActive(false);
    }

    public void TogglePanel(BaseEventData eventData)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        isExpanded = !isExpanded;
        currentCoroutine = StartCoroutine(AnimatePanel(isExpanded ? expandedSize : collapsedSize));
    }

    private IEnumerator AnimatePanel(Vector2 targetSize)
    {
        Vector2 startSize = rectTransform.sizeDelta;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize; // Ensure it reaches exact size

        // Show or hide the text based on the panel's state
        if (panelText != null)
        {
            panelText.gameObject.SetActive(isExpanded);
        }
    }
}
