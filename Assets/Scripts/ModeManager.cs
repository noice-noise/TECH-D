using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : Singleton<ModeManager> {

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
        im.onClick.AddListener(delegate { HandleModesChange(TechDMode.Interactive); });

        var tm = tourModeButton.GetComponent<Button>();
        tm.onClick.AddListener(delegate { HandleModesChange(TechDMode.Tour); });

        var pfm = pathFindingModeButton.GetComponent<Button>();
        pfm.onClick.AddListener(delegate { HandleModesChange(TechDMode.PathFinding); });

        CameraManager.OnCameraTargetChanged += HandleTargetChange;
    }

    private void HandleTargetChange() {
        switch(currentMode) {
            case TechDMode.Interactive:
                HandleInteractiveMode();
                break;
            case TechDMode.Tour:
                HandleTourMode();
                break;
            case TechDMode.PathFinding:
                HandlePathFindingMode();
                break;
            default:
                Debug.LogError("Mode invalid.");
                break;
        }
    }

    public void HandleModesChange(TechDMode newMode) {

        if (newMode == currentMode) return;

        HandleModeTermination(currentMode);
        currentMode = newMode;
        HandleModeStart(currentMode);
    }

    public void HandleModes() {
        switch(currentMode) {
            case TechDMode.Interactive:
                HandleInteractiveMode();
                break;
            case TechDMode.Tour:
                HandleTourMode();
                break;
            case TechDMode.PathFinding:
                HandlePathFindingMode();
                break;
            default:
                Debug.LogError("Mode invalid.");
                break;
        }
    }

    private void HandleModeStart(TechDMode toStart) {
        switch(toStart) {
            case TechDMode.Interactive:
                StartInteractiveMode();
                break;
            case TechDMode.Tour:
                StartTourMode();
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
                StopTourMode();
                break;
            case TechDMode.PathFinding:
                StopPathFindingMode();
                break;
            default:
                Debug.LogError("Mode invalid.");
                break;
        }
    }

    private void StartTourMode() {
        if (!TourMode.Instance.onTourMode) {
            TourMode.Instance.HandleTourModeToggle();
        }
    }

    private void StopTourMode() {
        if (TourMode.Instance.onTourMode) {
            TourMode.Instance.HandleTourModeToggle();
        }
    }

    public void StartInteractiveMode() {
        HandleInteractiveMode();
    }

    public void StopInteractiveMode() {
        
    }

    public void StartPathFindingMode() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
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
        
    }

    public void HandleInteractiveMode() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
    }

    public void HandlePathFindingMode() {
        
    }
}
