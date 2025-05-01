using UnityEngine;
using System.Collections;

public class MatchedCardAnimator : MonoBehaviour
{
    public float animationTime = 0.5f;
    public Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f);

    public IEnumerator AnimateToMatchPanel(Transform targetParent, Vector3 targetPosition)
    {
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        float elapsed = 0f;

        while (elapsed < animationTime)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / animationTime);
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / animationTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale = targetScale;

        transform.SetParent(targetParent, true); // Optional: parent to match panel
    }
}

