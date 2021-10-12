using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
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
