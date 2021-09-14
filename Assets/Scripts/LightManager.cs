using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {


    public bool handleLightSource;
    public Vector3 offset;
    private Vector3 baseRotation;

    public float smoothingRate = 0.2f;

    private void Awake() {
        baseRotation = transform.rotation.eulerAngles;
    }

    private void Update() {
        if (handleLightSource) {
            HandleLightSource();
        }
    }

    /// <summary>
    /// Handles Light rotation adjustment based on a target transform.
    /// </summary>
    private void HandleLightSource() {
        if (CameraManager.Instance.currentCameraState == CameraManager.CameraState.FocusedView) {

            Transform camera = CameraManager.Instance.focusedViewCamera.m_Follow;
            Transform focusTransform = camera.transform;
            Vector3 targetLightRotation = focusTransform.rotation.eulerAngles + offset;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetLightRotation), smoothingRate * Time.deltaTime);

        } else {
            transform.rotation = Quaternion.Euler(baseRotation);
        }
    }
}
