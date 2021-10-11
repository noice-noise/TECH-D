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

    public delegate void CameraTargetChanged();
    public static event CameraStateChanged OnCameraTargetChanged;


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

    /// <summary>
    /// Switch specified CinemachineVirtualCamera's target.
    /// 0 MapView, 1 TopView, 2 FocusedView
    /// </summary>
    /// <param name="camNumber"></param>
    /// <param name="newTarget"></param>
    public void SwitchVirtualCameraTarget(int camNumber, Transform newTarget) {
        switch (camNumber) {
            case 0:
                SwitchMapViewTarget(newTarget);
                break;
            case 1:
                SwitchTopViewTarget(newTarget);
                break;
            case 2:
                SwitchFocusedViewTarget(newTarget);
                break;
            default:
                Debug.LogError("Camera number invalid.");
                break;
        }
    }

    public void SwitchCameraTarget(Transform newTarget) {
        if (newTarget == null)
            return;

        if (autoSwitchCameraMode && currentCameraState == CameraState.MapView) {
            SwitchCameraMode(CameraState.TopView);
        }

        currentTarget = newTarget;
        
        SwitchTopViewTarget(newTarget);
        SwitchFocusedViewTarget(newTarget);

        if (OnCameraTargetChanged != null) {
            OnCameraTargetChanged();
        }
    }

    /// <summary>
    /// It is recommended not to use this method, for map view camera must not be changed outside editor.
    /// </summary>
    /// <param name="newTarget"></param>
    internal void SwitchMapViewTarget(Transform newTarget) {
        topViewCamera.m_Follow = newTarget;
        topViewCamera.m_LookAt = newTarget;
    }

    internal void SwitchTopViewTarget(Transform newTarget) {
        topViewCamera.m_Follow = newTarget;
        topViewCamera.m_LookAt = newTarget;
    }

    internal void SwitchFocusedViewTarget(Transform newTarget) {
        focusedViewCamera.m_Follow = newTarget;
        focusedViewCamera.m_LookAt = newTarget;
    }
}
