using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : Singleton<ModeManager> {
    // TODO This class needs refactoring, Modes-related methods should be delegated not handled.
    // Refactor goal: Only high-level mode change should be handled in this class.

    public enum TechDMode {
        Interactive, Tour, PathFinding
    }

    public PathFindingMode pathFindingMode;

    [Header("Indicator")]
    public GameObject modeIndicator;
    private TextMeshProUGUI modeIndicatorText;

    private TechDMode currentMode;

    [Header("ModeManager Components")]
    public Button interactiveModeButton;
    public Button tourModeButton;
    public Button pathFindingModeButton;

    [Header("Mode Components")]
    public GameObject tourModeIndicator;

    public GameObject[] disableObjectsOnTourMode;


    private void Awake() {

        pathFindingMode = pathFindingModeButton.gameObject.GetComponent<PathFindingMode>();

        var textTrans = modeIndicator.transform.Find("Text");
        modeIndicatorText = textTrans.GetComponent<TextMeshProUGUI>();

        tourModeIndicator.SetActive(false);

        InitModeButtonListeners();

        CameraManager.OnCameraTargetChanged += HandleTargetChange;
    }

    private void InitModeButtonListeners() {
        interactiveModeButton.onClick.AddListener(delegate { HandleModeChange(TechDMode.Interactive); });
        tourModeButton.onClick.AddListener(delegate { HandleModeChange(TechDMode.Tour); });
        pathFindingModeButton.onClick.AddListener(delegate { HandleModeChange(TechDMode.PathFinding); });
    }

    public void HandleModeChange(TechDMode newMode) {

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
                pathFindingMode.HandlePathFindingMode();
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
                HandleModeChange(TechDMode.Interactive);
                break;
            case 1:
                HandleModeChange(TechDMode.Tour);
                break;
            case 2:
                HandleModeChange(TechDMode.PathFinding);
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
                pathFindingMode.StartPathFindingMode();
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
                pathFindingMode.StopPathFindingMode();
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
        // no specific implementation aside from being the default
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

    public void HandleInteractiveMode() {
        CameraManager.Instance.autoSwitchCameraMode = true;
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
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
}
