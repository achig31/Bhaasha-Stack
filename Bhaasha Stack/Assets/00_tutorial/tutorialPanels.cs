using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPanels : MonoBehaviour
{
    public GameObject[] panels;
    private int currentIndex = 0;

    public void ShowNextPanel()
    {
        if (currentIndex < panels.Length - 1)
        {
            panels[currentIndex].SetActive(false);
            currentIndex++;
            panels[currentIndex].SetActive(true);
        }
    }
}
