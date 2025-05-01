using UnityEngine;
using System.Collections;

public class MatchedCardAnimator : MonoBehaviour
{
    public float animationDuration = 0.5f;

    public IEnumerator AnimateToSlot(Transform slotTransform, Vector3 localOffset)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = slotTransform.position + slotTransform.TransformVector(localOffset);

        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(0.5f, 0.5f, 1f); // Shrink a bit

        float t = 0f;

        while (t < animationDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t / animationDuration);
            transform.localScale = Vector3.Lerp(startScale, endScale, t / animationDuration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        transform.localScale = endScale;
        transform.SetParent(slotTransform, true);
    }
}
