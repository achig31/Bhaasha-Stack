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

    // New variables to show images
    public Image matchImage; // Image to display in the details panel
    public Sprite[] matchImages; // Array of sprites for the 6 pairs

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = collapsedSize;

        ToggleElements(false);
        if (closeButton != null) closeButton.SetActive(false);

        if (matchImage != null)
        {
            matchImage.gameObject.SetActive(false); // Hide image by default
        }
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

        while (elapsedTime < animationDuration)
        {
            rectTransform.sizeDelta = Vector2.Lerp(startSize, targetSize, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;

        ToggleElements(isExpanded);
        if (closeButton != null) closeButton.SetActive(isExpanded);

        // Show the image only after the panel has finished expanding
        if (isExpanded && matchImage != null)
        {
            matchImage.gameObject.SetActive(true); // Show image after animation
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