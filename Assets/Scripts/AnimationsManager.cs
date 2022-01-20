using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationsManager : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject summaryScreen;
    public PlayerControls pControls;

    public void DisableFadingImage()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }

    public void EnableFadingImage()
    {
        gameObject.GetComponent<Image>().enabled = true;
    }

    public void EnableSummary()
    {
        StartCoroutine(WaitTimer(1.5f, false, true));
        pControls.ToggleControls();
    }
    public void DisableAll()
    {
        StartCoroutine(WaitTimer(1.5f, false, false));
    }

    public void EnableMainMenu()
    {
        StartCoroutine(WaitTimer(1.5f, true, false));
    }

    public IEnumerator WaitTimer(float seconds, bool mmBool, bool sumBool)
    {
        yield return new WaitForSeconds(seconds);
        mainMenuScreen.SetActive(mmBool);
        summaryScreen.SetActive(sumBool);
    }

    public void FixTimeScale()
    {
        Time.timeScale = 1;
    }
}
