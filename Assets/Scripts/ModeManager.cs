using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : Singleton<ModeManager> {

    public enum TechDMode {
        Interactive, Tour, PathFinding
    }

    [SerializeField] private TechDMode currentMode;

    public GameObject modeIndicator;
    private TextMeshProUGUI modeIndicatorText;

    public GameObject interactiveModeButton;
    public GameObject tourModeButton;
    public GameObject pathFindingModeButton;

    public GameObject navMeshAgent;
    public GameObject pathFindingControls;


    private void Awake() {

        var textTrans = modeIndicator.transform.Find("Text");
        modeIndicatorText = textTrans.GetComponent<TextMeshProUGUI>();

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
                modeIndicatorText.text = "Interactive Mode";
                break;
            case TechDMode.Tour:
                HandleTourMode();
                modeIndicatorText.text = "Tour Mode";
                break;
            case TechDMode.PathFinding:
                HandlePathFindingMode();
                modeIndicatorText.text = "Path Finding Mode";
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

    private void HandleModeStart(TechDMode toStart) {
        switch(toStart) {
            case TechDMode.Interactive:
                StartInteractiveMode();
                modeIndicatorText.text = "Interactive Mode";
                break;
            case TechDMode.Tour:
                StartTourMode();
                modeIndicatorText.text = "Tour Mode";
                break;
            case TechDMode.PathFinding:
                StartPathFindingMode();
                modeIndicatorText.text = "Path Finding Mode";
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
            CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
        }
    }

    private void StopTourMode() {
        if (TourMode.Instance.onTourMode) {
            TourMode.Instance.HandleTourModeToggle();
            CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        }
    }

    public void StartInteractiveMode() {
        HandleInteractiveMode();
    }

    public void StopInteractiveMode() {
        
    }

    public void StartPathFindingMode() {
        navMeshAgent.SetActive(true);
        pathFindingControls.SetActive(true);
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        AgentController.Instance.StartAgentBehavior();
    }

    public void StopPathFindingMode() {
        AgentController.Instance.StopAgentBehavior();
        pathFindingControls.SetActive(false);
    }

    public void FollowAgent() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        CameraManager.Instance.SwitchTopViewTarget(navMeshAgent.transform);
    }

    public void FollowAgentViaFocused() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
        CameraManager.Instance.SwitchFocusedViewTarget(navMeshAgent.transform.Find("Follow"));
    }

    public void HandleTourMode() {
        
    }

    public void HandleInteractiveMode() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
    }

    public void HandlePathFindingMode() {
        FollowAgentViaFocused();
    }
}
