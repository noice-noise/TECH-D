using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour {

    [SerializeField] private TechDMode currentMode;

    public GameObject interactiveModeButton;
    public GameObject tourModeButton;
    public GameObject pathFindingModeButton;

    private void Awake() {
        var im = interactiveModeButton.GetComponent<Button>();
        im.onClick.AddListener(delegate { HandleInteractiveMode(); });

        var tm = tourModeButton.GetComponent<Button>();
        tm.onClick.AddListener(delegate { HandleTourMode(); });

        var pfm = pathFindingModeButton.GetComponent<Button>();
        pfm.onClick.AddListener(delegate { HandlePathFindingMode(); });
    }

    public enum TechDMode {
        Interactive, Tour, PathFinding
    }

    public void HandleModes() {
        switch(currentMode) {
            case TechDMode.Interactive:
                HandleInteractiveMode();
                break;
            case TechDMode.Tour:
                HandleInteractiveMode();
                break;
            case TechDMode.PathFinding:
                HandleInteractiveMode();
                break;
            default:
                break;
        }
    }

    public void HandleInteractiveMode() {
        // auto focusedview, left right building nav, disables pathfinding
        Debug.Log("1");
        currentMode = TechDMode.Interactive;
    }

    public void HandleTourMode() {
        // run tour animation (path finding modifed)
        Debug.Log("2");
        TourMode.Instance.ToggleTourMode();
        currentMode = TechDMode.Tour;
    }

    public void HandlePathFindingMode() {
        // disables auto focused view, allows follow agent, 
        Debug.Log("3");
        currentMode = TechDMode.PathFinding;
    }
}
