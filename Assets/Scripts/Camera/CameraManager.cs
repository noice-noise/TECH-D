using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class CameraManager : Singleton<CameraManager>
{
    public enum CameraState {
        MapView, TopView, FocusedView
    }

    [Header("Cameras")]
    public CinemachineVirtualCamera mapViewCamera;
    public CinemachineVirtualCamera topViewCamera;
    public CinemachineVirtualCamera focusedViewCamera;

    [Header("Camera Flags")]
    public Transform centerTarget;
    public Transform currentTarget;
    public CameraState currentCameraState;

    [Header("Camera Settings")]
    public bool autoSwitchCameraMode = true;

    private Animator animator;

    public delegate void CameraStateChanged();

    public static event CameraStateChanged OnCameraStateChanged;


    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void SwitchCameraModeWith(int stateNumber) {
        CameraState newState = currentCameraState;

        switch (stateNumber) {
            case 0:
                newState = CameraState.MapView;
                break;
            case 1:
                newState = CameraState.TopView;
                break;
            case 2:
                newState = CameraState.FocusedView;
                break;
            default:
                Debug.LogError("Camera State value invalid.");
                break;
        }

        if (newState != currentCameraState) {
            SwitchCameraMode(newState);
        }
    }

    public void SwitchCameraMode(CameraState cameraState) {
        animator.Play(cameraState.ToString());
        currentCameraState = cameraState;

        if (OnCameraStateChanged != null) {
            OnCameraStateChanged();
        }
    }

    public void SwitchCameraTarget(Transform newTarget) {
        if (newTarget == null)
            return;

        if (autoSwitchCameraMode && currentCameraState == CameraState.MapView)
            SwitchCameraMode(CameraState.TopView);

        currentTarget = newTarget;
        topViewCamera.m_Follow = newTarget;
        topViewCamera.m_LookAt = newTarget;
        focusedViewCamera.m_Follow = newTarget;
        focusedViewCamera.m_LookAt = newTarget;
    }


}
