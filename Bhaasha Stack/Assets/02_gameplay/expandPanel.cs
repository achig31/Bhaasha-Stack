using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public Image matchImage; 
    public Sprite[] matchImages; 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = collapsedSize;

        ToggleElements(false);
        if (closeButton != null) closeButton.SetActive(false);

        if (matchImage != null)
        {
            matchImage.gameObject.SetActive(false); 
        }
    }

    public void TogglePanel(BaseEventData eventData)
    {
        if (!isInteractable) return;
        TogglePanelInternal(!isExpanded);
    }

    private void TogglePanelInternal(bool expand)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        isExpanded = expand;
        currentCoroutine = StartCoroutine(AnimatePanel(isExpanded ? expandedSize : collapsedSize));
    }

    public void ExpandPanelExternally()
    {
        if (!isExpanded)
        {
            TogglePanelInternal(true);
        }
    }

    public void CollapsePanelExternally()
    {
        if (isExpanded)
        {
            TogglePanelInternal(false);
        }
    }
    private IEnumerator AnimatePanel(Vector2 targetSize)
    {
        Vector2 startSize = rectTransform.sizeDelta;
        float elapsedTime = 0f;

        if (!isExpanded)
        {
            ToggleElements(false);
            if (closeButton != null) closeButton.SetActive(false);
        }

        while (elapsedTime < animationDuration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;

        if (isExpanded)
        {
            ToggleElements(true);
            if (closeButton != null) closeButton.SetActive(true);
            if (matchImage != null) matchImage.gameObject.SetActive(true); 
        }
        else
        {
            if (matchImage != null) matchImage.gameObject.SetActive(false);
        }
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

    // Method to set the match image
    public void ShowMatchImage(int matchIndex)
    {
        if (matchImage != null && matchImages != null && matchImages.Length > matchIndex)
        {
            matchImage.sprite = matchImages[matchIndex];
        }
    }
}