using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.AI;

public class Navigation : Singleton<Navigation> {


    private Camera mainCamera;
    
    [Header("Variables")]
    public Transform currentlyFocusedBuilding; // note that camera focus and building focus are different
    private bool selectButtonPressed;

    [Header("Cursor")]
    public Texture2D normalCursor;
    public Texture2D selectableCursor;

    public Transform navMeshAgentTransform;
    public AgentController agent;


    private void Awake() {
        mainCamera = Camera.main;
        agent = navMeshAgentTransform.GetComponent<AgentController>();
    }

    private void Update() {
        HandleInputs();
        HandleSelection();
    }

    public void HandleButtonClickedEvent(GameObject obj) {
        if (obj == null) {
            Debug.LogError("Null parameter on button onClicked event.");
        }

        if (obj.name.Equals("MapView")) {
            SwitchMapView();
        } else if (obj.name.Equals("TopView")) {
            SwitchTopView();
        } else if (obj.name.Equals("FocusedView")) {
            SwitchFocusedView();
        }
    }

    private void HandleSelection() {

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            var selected = hit.collider;
            // agent.SetAgentDestination(hit.point);
            if (selected != null && selected.gameObject.layer == LayerMask.NameToLayer("SelectableBuilding")) {
                SetCursorSelectable();
                if (selectButtonPressed) {

                    agent.SetAgentDestination(hit.point);

                    var toFocus = FindParentBuilding(hit.transform).Find("Focus");
                    if (toFocus == null) {
                        Debug.Log("No focusable object.");
                        return;
                    }
                    SelectAndUpdateUI(toFocus);
                }
            } else {
                SetCursorNormal();
            }
        }
    }

    /// <summary>
    /// Selects a new targetTransform to prompt focus.
    /// All navigation-related target will get the primary parent "Building" as reference, 
    /// while camera will get the "Focus" as a target.
    /// </summary>
    public void SelectAndUpdateUI(Transform targetTransform) {
        if (targetTransform.CompareTag("SelectableBuilding")) {
            this.currentlyFocusedBuilding = targetTransform;
            var focus = targetTransform.Find("Focus");
            if (focus != null) {
                targetTransform = focus;
            }
        }
        else if (targetTransform.parent.CompareTag("SelectableBuilding")) {
            this.currentlyFocusedBuilding = targetTransform.parent;
        } else {
            this.currentlyFocusedBuilding = targetTransform;
        }

        CameraManager.Instance.SwitchCameraTarget(targetTransform);
        UIManager.Instance.UpdateMapUI();
    }
 
    private Transform FindParentBuilding (Transform targetTransform) {

        while (!targetTransform.CompareTag("SelectableBuilding")) {
            targetTransform = targetTransform.parent;
        }

        return targetTransform;
    }

    public void SetCursorSelectable() {
        Cursor.SetCursor(selectableCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorNormal() {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    private void HandleInputs() {
        if (Input.GetKey(KeyCode.F1))
        {
            SwitchMapView();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SwitchTopView();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SwitchFocusedView();
        }

        if (Input.GetMouseButtonDown(0)) {
            selectButtonPressed = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            selectButtonPressed = false;
        }
    }

    private void SwitchFocusedView() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
    }

    private void SwitchTopView() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
    }

    private void SwitchMapView() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.MapView);
    }
}

