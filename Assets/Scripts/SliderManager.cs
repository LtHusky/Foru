using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Pump pumpScript;
    public Slider slider;

    void Start()
    {
        slider.onValueChanged.AddListener((val) => 
        {
            pumpScript.SetSliderSpeed(val);
        });
    }

    public void DisableInteraction()
    {
        slider.interactable = false;
        slider.value = 0;
    }

    public void EnableInteraction()
    {
        slider.interactable = true;
    }
}
