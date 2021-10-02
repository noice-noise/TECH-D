using UnityEngine;
using Cinemachine;
using System;

public class FocusedViewCamera : MonoBehaviour {

    public Vector3 adjustedFollowOffset = Vector3.zero;
    public Vector3 baseFollowOffset;

    public Vector3 adjustedTrackedObjectOffset = Vector3.zero;
    public Vector3 baseTrackedObjectOffset;

    private CinemachineVirtualCamera focusedViewCamera;
    private Transform currentTarget;
    public bool allowOffset;

    private void Awake() {
        focusedViewCamera = GetComponent<CinemachineVirtualCamera>();
        baseFollowOffset =  focusedViewCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        baseTrackedObjectOffset =  focusedViewCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
    }

    private void Update() {
        currentTarget = focusedViewCamera.m_Follow;

        if (allowOffset) {
            HandleFocusedOffset();
        }
    }

    private void HandleFocusedOffset() {
        if (currentTarget != null && !currentTarget.CompareTag("FocusedViewTarget")) {
            AdjustAimOffset(focusedViewCamera, adjustedFollowOffset);
            AdjustTrackedOffset(focusedViewCamera, adjustedTrackedObjectOffset);
        } else {
            RestoreAimOffset(focusedViewCamera, baseFollowOffset);
            RestoreTrackedOffset(focusedViewCamera, baseTrackedObjectOffset);
        }
    }

    private void AdjustAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        Debug.Log("Offset adjusted");
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = offset;
    }

    private void RestoreAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        Debug.Log("Offset restored");
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = offset;
    }

    private void AdjustTrackedOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        Debug.Log("Offset adjusted");
        var camComposer = camera.GetCinemachineComponent<CinemachineComposer>();
        camComposer.m_TrackedObjectOffset = offset;
    }

    private void RestoreTrackedOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        Debug.Log("Offset restored");
        var camComposer = camera.GetCinemachineComponent<CinemachineComposer>();
        camComposer.m_TrackedObjectOffset = offset;
    }
}