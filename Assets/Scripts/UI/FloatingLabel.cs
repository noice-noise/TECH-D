using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLabel : MonoBehaviour {

    public bool allowShowAndHide = true;

    private void OnEnable() {
        CameraManager.OnCameraStateChanged += HandleCameraStateChange;
    }

    private void OnDestroy() {
        CameraManager.OnCameraStateChanged -= HandleCameraStateChange;
    }

    public void HandleCameraStateChange() {
        if (CameraManager.Instance.currentCameraState == CameraManager.CameraState.FocusedView) {
            if (allowShowAndHide) {
                HideLabel();
            }
        } else {
            if (allowShowAndHide) {
                if (gameObject.activeSelf == false) {
                    ShowLabel();
                }
            }
        }
    }

    private void ShowLabel() {
        gameObject.SetActive(true);
    }

    private void HideLabel() {
        gameObject.SetActive(false);
    }
}
