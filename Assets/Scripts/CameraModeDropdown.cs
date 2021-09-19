using UnityEngine;
using TMPro;
using System;

public class CameraModeDropdown : MonoBehaviour {
    public int currentValue;

    private TMP_Dropdown dropdown;

    private void Awake() {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Start() {
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    private void Update() {
        HandleCameraState();
    }

    private void HandleCameraState() {
        CameraManager.CameraState camState = CameraManager.Instance.currentCameraState;
        
        switch (camState) {
            case CameraManager.CameraState.MapView:
                dropdown.value = 0;
                break;
            case CameraManager.CameraState.TopView:
                dropdown.value = 1;
                break;
            case CameraManager.CameraState.FocusedView:
                dropdown.value = 2;
                break;
            default:
                Debug.LogError("Invalid camera state.");
                break;
        }
    }


    public void DropdownValueChanged() {
        CameraManager.Instance.SwitchCameraModeWith(dropdown.value);
    }
}