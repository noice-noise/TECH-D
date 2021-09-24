using UnityEngine;
using Cinemachine;
using System;

public class FocusedViewCamera : MonoBehaviour {

    public Vector3 focusedViewOffset = Vector3.zero;
    public Vector3 baseOffset;

    private CinemachineVirtualCamera focusedViewCamera;
    private Transform currentTarget;
    public bool allowOffset;

    private void Awake() {
        focusedViewCamera = GetComponent<CinemachineVirtualCamera>();
        baseOffset =  focusedViewCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    private void Update() {
        currentTarget = focusedViewCamera.m_Follow;

        if (allowOffset) {
            HandleFocusedOffset();
        }
    }

    private void HandleFocusedOffset() {
        if (currentTarget != null && !currentTarget.CompareTag("FocusedViewTarget")) {
            AdjustAimOffset(focusedViewCamera, focusedViewOffset);
        } else {
            RestoreAimOffset(focusedViewCamera, baseOffset);
        }
    }

    private void AdjustAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        Debug.Log("Offset adjusted");
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = baseOffset + offset;
    }

    private void RestoreAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        Debug.Log("Offset restored");
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = baseOffset;
    }
}