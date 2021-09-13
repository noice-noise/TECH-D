using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{

    public Quaternion offset;
    public Vector3 degrees;
    public Transform original;

    public bool handleLightSource;

    private void Start() {
        original = transform;
    }

    private void Update() {
        if (handleLightSource) {
            HandleLightSource();
        }
    }

    private void HandleLightSource() {
        if (CameraManager.Instance.currentCameraState == CameraManager.CameraState.FocusedView) {
            Debug.Log("Looking...");
            // transform.rotation = CameraManager.Instance.currentTarget.rotation;
            // transform.rotation = offset;

            // transform.position = CameraManager.Instance.focusedViewCamera .transform.position;
            // transform.LookAt(CameraManager.Instance.currentTarget);
            // transform.LookAt()
            Quaternion source = Camera.main.transform.rotation;
            // transform.rotation = 
            // new Quaternion(source.x + offset.x, source.y + offset.y, source.z + offset.z, source.w + offset.w);

            transform.localRotation = Quaternion.Euler(Camera.main.transform.forward + degrees);

        } else {
            transform.localRotation = original.localRotation;
        }
    }
}
