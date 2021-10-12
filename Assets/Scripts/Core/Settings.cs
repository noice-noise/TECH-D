using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Singleton<Settings> {

    private Resolution[] resolutions;

    private void Start() {
        resolutions =  Screen.resolutions;
    }

    private void Update() {
        HandleKeyInputs();
    }

    private void HandleKeyInputs() {

    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex) {
        Debug.Log("Quality set to: " + qualityIndex + ": " + QualitySettings.names);
        QualitySettings.SetQualityLevel(qualityIndex, true);
    }

}
