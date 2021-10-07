using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour {

    public enum TechDMode {
        Interactive, Tour, PathFinding
    }

    [SerializeField] private TechDMode currentMode;

    public GameObject interactiveModeButton;
    public GameObject tourModeButton;
    public GameObject pathFindingModeButton;

    public GameObject navMeshAgent;

    private void Awake() {
        var im = interactiveModeButton.GetComponent<Button>();
        im.onClick.AddListener(delegate { HandleModes(TechDMode.Interactive); });

        var tm = tourModeButton.GetComponent<Button>();
        tm.onClick.AddListener(delegate { HandleModes(TechDMode.Tour); });

        var pfm = pathFindingModeButton.GetComponent<Button>();
        pfm.onClick.AddListener(delegate { HandleModes(TechDMode.PathFinding); });

        // AgentController navMeshAgent.GetComponent<AgentController>();
    }

    public void HandleModes(TechDMode newMode) {
        HandleModeTermination(currentMode);
        currentMode = newMode;


        switch(newMode) {
            case TechDMode.Interactive:
                StopInteractiveMode();
                break;
            case TechDMode.Tour:
                StopInteractiveMode();
                break;
            case TechDMode.PathFinding:
                StartPathFindingMode();
                break;
            default:
                Debug.LogError("Mode invalid.");
                break;
        }
    }

    private void HandleModeTermination(TechDMode toTerminate) {

        switch(toTerminate) {
            case TechDMode.Interactive:
                StopInteractiveMode();
                break;
            case TechDMode.Tour:
                StopInteractiveMode();
                break;
            case TechDMode.PathFinding:
                StopPathFindingMode();
                break;
            default:
                Debug.LogError("Mode invalid.");
                break;
        }
    }


    public void StartInteractiveMode() {

    }

    public void StopInteractiveMode() {

    }

    public void StartPathFindingMode() {
        AgentController.Instance.StartAgentBehavior();
    }

    public void StopPathFindingMode() {
        AgentController.Instance.StopAgentBehavior();
    }

    public void FollowAgent() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        CameraManager.Instance.SwitchTopViewTarget(navMeshAgent.transform);
    }

    public void HandleTourMode() {
        TourMode.Instance.HandleTourModeToggle();
    }
}
