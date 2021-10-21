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

    public bool onInteractiveMode;
    public PathFindingMode pathFindingMode;

    [Header("Indicator")]
    public GameObject modeIndicator;
    private TextMeshProUGUI modeIndicatorText;
    public GameObject messageIndicator;
    private TextMeshProUGUI messageIndicatorText;

    private TechDMode currentMode;

    [Header("ModeManager Components")]
    public Button interactiveModeButton;
    public Button tourModeButton;
    public Button pathFindingModeButton;

    [Header("Mode Components")]
    public GameObject tourModeIndicator;


    public GameObject[] disableObjectsOnInteractive;
    public GameObject[] disableObjectsOnTourMode;
    public GameObject[] disableObjectsOnPathFindingMode;


    private void Awake() {

        pathFindingMode = pathFindingModeButton.gameObject.GetComponent<PathFindingMode>();

        var textTrans = modeIndicator.transform.Find("Text");
        modeIndicatorText = textTrans.GetComponent<TextMeshProUGUI>();

        textTrans = messageIndicator.transform.Find("Text");
        messageIndicatorText = textTrans.GetComponent<TextMeshProUGUI>();

        InitModeButtonListeners();
        CameraManager.OnCameraTargetChanged += HandleTargetChange;
    }

    private void Start() {
        InitModeManager();
    }

    public void InitModeManager() {
        StartInteractiveMode();
        tourModeIndicator.SetActive(false);
    }

    /// <summary>
    /// Interactive = 0, Tour = 1, PathFinding = 2
    /// </summary>
    /// <param name="modeInt"></param>
    public void RequestModeChange(int modeInt) {
        TechDMode requestModeValue;

        switch (modeInt)
        {
            case 0:
                requestModeValue = TechDMode.Interactive;
                break;
            case 1:
                requestModeValue = TechDMode.Tour;
                break;
            case 2:
                requestModeValue = TechDMode.PathFinding;
                break;
            default:
                requestModeValue = TechDMode.Interactive;
                break;
        }

        if (currentMode != requestModeValue) {
            HandleModeChange(requestModeValue);
        }
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
        HandleObjectDisabling();
    }

    private void HandleObjectDisabling() {
        DisableObjectsDuring(onInteractiveMode, disableObjectsOnInteractive);
        DisableObjectsDuring(TourMode.Instance.onTourMode, disableObjectsOnTourMode);
        DisableObjectsDuring(pathFindingMode.onPathFindingMode, disableObjectsOnPathFindingMode);
    }

    private void DisableObjectsDuring(bool modeActive, GameObject[] objList) {
        if (modeActive) {
            if (objList.Length > 0) {
                foreach (var item in objList) {
                    item.SetActive(false);
                }
            }
        } else {
            if (objList.Length > 0) {
                foreach (var item in objList) {
                    item.SetActive(true);
                }
            }
        }
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
                messageIndicatorText.text = "Select a building.";
                break;
            case TechDMode.Tour:
                StartTourMode();
                modeIndicatorText.text = "Tour Mode";
                tourModeButton.interactable = false;
                messageIndicatorText.text = "Select another mode to stop Tour Mode.";
                break;
            case TechDMode.PathFinding:
                pathFindingMode.StartPathFindingMode();
                pathFindingModeButton.interactable = false;
                modeIndicatorText.text = "Path Finding Mode";
                messageIndicatorText.text = "Press or Hold [RIGHT] Mouse Button to freely walk anywhere.";
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

    private void StartInteractiveMode() {
        onInteractiveMode = true;
        HandleInteractiveMode();
    }

    private void StopInteractiveMode() {
        onInteractiveMode = true;
    }

    private void StartTourMode() {
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

    private void HandleInteractiveMode() {
        CameraManager.Instance.autoSwitchCameraMode = true;
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
    }

    private void HandleTourMode() {

    }

    public bool OnInteractiveMode() {
        return onInteractiveMode;
    }

    public bool OnTourMode() {
        return TourMode.Instance.onTourMode;

    }

    public bool OnPathFindingMode() {
        return pathFindingMode.onPathFindingMode;
    }
}
