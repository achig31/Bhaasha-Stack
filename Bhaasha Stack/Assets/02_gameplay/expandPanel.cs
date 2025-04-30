using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ExpandPanel : MonoBehaviour
{
    public Vector2 expandedSize = new Vector2(200, 305);
    public Vector2 collapsedSize = new Vector2(35, 305);
    public float animationDuration = 0.3f;
    public bool isInteractable = true;

    private bool isExpanded = false;
    private RectTransform rectTransform;
    private Coroutine currentCoroutine;

    // Support for multiple elements to toggle visibility
    public List<GameObject> panelElementsToToggle;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = collapsedSize; // Start collapsed

        // Hide all elements initially
        if (panelElementsToToggle != null)
        {
            foreach (var element in panelElementsToToggle)
            {
                if (element != null) element.SetActive(false);
            }
        }
    }

    public void TogglePanel(BaseEventData eventData)
    {
        if (!isInteractable) return;

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
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize; // Final adjustment

        // Show or hide all elements based on panel state
        if (panelElementsToToggle != null)
        {
            foreach (var element in panelElementsToToggle)
            {
                if (element != null) element.SetActive(isExpanded);
            }
        }
    }
}
