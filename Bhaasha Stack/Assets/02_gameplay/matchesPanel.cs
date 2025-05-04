using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchesPanel : MonoBehaviour
{
    public Transform[] matchSlots;
    private int currentSlotIndex = 0;

    public Transform GetNextSlot()
    {
        if (currentSlotIndex >= matchSlots.Length)
        {
            Debug.LogWarning("No more match slots available!");
            return null;
        }

        return matchSlots[currentSlotIndex++];
    }
}
