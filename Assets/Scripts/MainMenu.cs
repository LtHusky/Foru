using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject fadeImage;
    public Animator mmAnimator;

    // Fix timeScale for animation
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        fadeImage.GetComponent<Image>().enabled = true;
        mmAnimator.SetTrigger("FadeIn");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
}
