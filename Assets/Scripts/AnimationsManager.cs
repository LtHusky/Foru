using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    public GameObject endScreen;

    public void EnableSummaryMenu()
    {
        endScreen.SetActive(true);
    }

    public void DisableSummaryMenu()
    {
        endScreen.SetActive(false);
    }
}
