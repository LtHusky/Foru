using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject standardCameraView;
    public GameObject expandedCameraView;

    public void ExpandCameraView()
    {
        standardCameraView.SetActive(false);
        expandedCameraView.SetActive(true);
    }

    public void ResetCameraView()
    {
        standardCameraView.SetActive(true);
        expandedCameraView.SetActive(false);
    }
}
