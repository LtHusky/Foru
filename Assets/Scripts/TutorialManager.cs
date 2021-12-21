using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPages;
    int currentPage;

    public void NextPage()
    {
        if (currentPage < tutorialPages.Length - 1)
        {
            tutorialPages[currentPage].SetActive(false);
            tutorialPages[currentPage + 1].SetActive(true);
            currentPage++;
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            tutorialPages[currentPage].SetActive(false);
            tutorialPages[currentPage - 1].SetActive(true);
            currentPage--;
        }
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
    }
}
