using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : Singleton<SystemManager> {

    public GameObject startObject;
    public GameObject welcomeFrame;
    public GameObject[] mainControls;

    private void Start() {
        SelectStartingView();
        InitWelcomeFrame();
        DisableMainControls();
    }

    private void InitWelcomeFrame() {
        if (welcomeFrame != null && welcomeFrame.activeSelf == false) {
            welcomeFrame.SetActive(true);
        }
    }

    public void DisableMainControls() {
        if (mainControls.Length > 0) {
            foreach (var item in mainControls) {
                item.SetActive(false);
            }
        }
    }

    public void EnableMainControls() {
        if (mainControls.Length > 0) {
            foreach (var item in mainControls) {
                item.SetActive(true);
            }
        }
    }

    private void SelectStartingView() {
        Navigation.Instance.SelectAndUpdateUI(startObject.transform);
    }

    public void RestartSystem() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 
}
