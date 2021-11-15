using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void DisableFadingImage()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
