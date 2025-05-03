using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class disableImage : MonoBehaviour
{
    public Image imageToDisable; // Reference to the Image component

    // This function can be assigned to a Button's OnClick event
    public void DisableImageOnClick()
    {
        if (imageToDisable != null)
        {
            imageToDisable.enabled = false; // Disable the Image component
        }
    }
}
