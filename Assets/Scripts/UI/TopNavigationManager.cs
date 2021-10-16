using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopNavigationManager : MonoBehaviour {
    
    public Transform homePanel;
    public Transform mapPanel;
    public Transform queriesPanel;

    public TopNavButtons navButton;

    public Transform currentlyActivePanel; 

    private void Awake() {

        InitListeners(navButton.Home);
        InitListeners(navButton.Map);
        InitListeners(navButton.Queries);
    }

    private void Start() {
        InitPanel();
    }

    private void InitPanel() {
        homePanel.gameObject.SetActive(false);
        mapPanel.gameObject.SetActive(true);
        queriesPanel.gameObject.SetActive(false);
    }

    private void InitListeners(GameObject navButton) {
        navButton.GetComponent<Button>().onClick.AddListener( 
            delegate { 
                SelectionHandler(navButton);
            });
    }

    public void SelectionHandler(GameObject button) {
        if (ModeManager.Instance.OnTourMode()) {
            ModeManager.Instance.RequestModeChange(0);
        }

        if (button != null) {
            if (button.Equals(navButton.Home)) {
                SetCurrentActivePanel(homePanel);
            } else if (button.Equals(navButton.Map)) {
                SetCurrentActivePanel(mapPanel);
            } else if (button.Equals(navButton.Queries)) {
                SetCurrentActivePanel(queriesPanel);
            }
        }
    }

    private void SetCurrentActivePanel(Transform newCurrentPanel) {
        if (currentlyActivePanel == null || newCurrentPanel == null) {
            currentlyActivePanel = mapPanel;
        }

        currentlyActivePanel.gameObject.SetActive(false);
        newCurrentPanel.gameObject.SetActive(true);
        currentlyActivePanel = newCurrentPanel;
    }
}

[System.Serializable]
public class TopNavButtons {
    public GameObject Home;
    public GameObject Map;
    public GameObject Queries;
} 
