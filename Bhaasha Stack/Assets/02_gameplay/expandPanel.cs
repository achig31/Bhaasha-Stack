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

    public List<GameObject> panelElementsToToggle;
    public GameObject closeButton;
    public bool IsExpanded => isExpanded;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = collapsedSize;

        ToggleElements(false);
        if (closeButton != null) closeButton.SetActive(false);
    }

    // Called by EventTrigger for hover-based interaction (e.g., menu panel)
    public void TogglePanel(BaseEventData eventData)
    {
        if (!isInteractable) return;
        TogglePanelInternal(!isExpanded);
    }

    // Called by other scripts (e.g., GameManagerCard) to expand programmatically
    public void ExpandPanelExternally()
    {
        if (!isExpanded)
        {
            TogglePanelInternal(true);
        }
    }

    // Called by button or script to collapse
    public void CollapsePanelExternally()
    {
        if (isExpanded)
        {
            TogglePanelInternal(false);
        }
    }

    private void TogglePanelInternal(bool expand)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        isExpanded = expand;
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

        rectTransform.sizeDelta = targetSize;

        ToggleElements(isExpanded);
        if (closeButton != null) closeButton.SetActive(isExpanded);
    }

    private void ToggleElements(bool show)
    {
        if (panelElementsToToggle != null)
        {
            foreach (var element in panelElementsToToggle)
            {
                if (element != null) element.SetActive(show);
            }
        }
    }
}
