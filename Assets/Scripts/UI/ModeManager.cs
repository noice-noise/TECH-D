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

    [Header("Indicator")]
    public GameObject modeIndicator;
    private TextMeshProUGUI modeIndicatorText;

    private TechDMode currentMode;

    [Header("ModeManager Components")]
    public Button interactiveModeButton;
    public Button tourModeButton;
    public Button pathFindingModeButton;

    [Header("Mode Components")]
    public GameObject navMeshAgent;
    public GameObject pathFindingIndicator;
    public GameObject tourModeIndicator;

    public GameObject[] disableObjectsOnTourMode;


    private void Awake() {

        var textTrans = modeIndicator.transform.Find("Text");
        modeIndicatorText = textTrans.GetComponent<TextMeshProUGUI>();

        if (navMeshAgent == null) {
            navMeshAgent = GameObject.FindGameObjectWithTag("Agent");
        }

        pathFindingIndicator.SetActive(false);
        tourModeIndicator.SetActive(false);

        InitModeButtonListeners();

        CameraManager.OnCameraTargetChanged += HandleTargetChange;
    }

    private void InitModeButtonListeners() {
        interactiveModeButton.onClick.AddListener(delegate { HandleModesChange(TechDMode.Interactive); });
        tourModeButton.onClick.AddListener(delegate { HandleModesChange(TechDMode.Tour); });
        pathFindingModeButton.onClick.AddListener(delegate { HandleModesChange(TechDMode.PathFinding); });
    }

    public void HandleModesChange(TechDMode newMode) {

        if (newMode == currentMode) {
            return;
        }

        HandleModeTermination(currentMode);
        currentMode = newMode;
        HandleModeStart(currentMode);
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

    public void HandleModesChangeWithInt(int newModeCount) {
        switch(newModeCount) {
            case 0:
                HandleModesChange(TechDMode.Interactive);
                break;
            case 1:
                HandleModesChange(TechDMode.Tour);
                break;
            case 2:
                HandleModesChange(TechDMode.PathFinding);
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
                modeIndicatorText.text = "Interactive Mode";
                interactiveModeButton.interactable = false;
                break;
            case TechDMode.Tour:
                StartTourMode();
                modeIndicatorText.text = "Tour Mode";
                tourModeButton.interactable = false;
                break;
            case TechDMode.PathFinding:
                StartPathFindingMode();
                modeIndicatorText.text = "Path Finding Mode";
                pathFindingModeButton.interactable = false;
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
                interactiveModeButton.interactable = true;
                break;
            case TechDMode.Tour:
                StopTourMode();
                tourModeButton.interactable = true;
                break;
            case TechDMode.PathFinding:
                StopPathFindingMode();
                pathFindingModeButton.interactable = true;
                break;
            default:
                Debug.LogError("Mode invalid.");
                break;
        }
    }

    public void StartInteractiveMode() {
        HandleInteractiveMode();
    }

    public void StopInteractiveMode() {
        
    }

    public void StartTourMode() {
        if (!TourMode.Instance.onTourMode) {
            tourModeIndicator.SetActive(true);
            TourMode.Instance.HandleTourModeToggle();
            CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
        }
    }

    public void StopTourMode() {
        if (TourMode.Instance.onTourMode) {
            tourModeIndicator.SetActive(false);
            TourMode.Instance.HandleTourModeToggle();
            CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
            HandleTourMode();
        }
    }

    public void StartPathFindingMode() {
        navMeshAgent.SetActive(true);
        pathFindingIndicator.SetActive(true);
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        AgentController.Instance.StartAgentBehavior();
    }

    public void StopPathFindingMode() {
        AgentController.Instance.StopAgentBehavior();
        pathFindingIndicator.SetActive(false);
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
        if (TourMode.Instance.onTourMode) {
            if (disableObjectsOnTourMode.Length > 0) {
                foreach (var item in disableObjectsOnTourMode) {
                    item.SetActive(false);
                }
            }
        } else {
            if (disableObjectsOnTourMode.Length > 0) {
                foreach (var item in disableObjectsOnTourMode) {
                    item.SetActive(true);
                }
            }
        }
    }

    public void HandleInteractiveMode() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
    }

    public void HandlePathFindingMode() {
        // FollowAgentViaFocused();
    }
}
