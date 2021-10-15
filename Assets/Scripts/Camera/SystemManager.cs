using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : Singleton<SystemManager> {

    public GameObject startObject;

    private void Start() {
        SelectStartingView();
    }

    private void SelectStartingView() {
        Navigation.Instance.SelectAndUpdateUI(startObject.transform);
    }

    public void RestartSystem() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 
}
