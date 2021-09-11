using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Navigation : Singleton<Navigation>
{


    private Camera mainCamera;
    
    [Header("Variables")]
    public Transform currentlyFocusedBuilding; // note that camera focus and building focus are different
    [SerializeField] private bool selectButtonPressed;

    [Header("Cursor")]
    public Texture2D normalCursor;
    public Texture2D selectableCursor;


    private void Awake() {
        mainCamera = Camera.main;
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
            if (selected != null && selected.gameObject.layer == LayerMask.NameToLayer("SelectableBuilding")) {
                SetCursorSelectable();
                if (selectButtonPressed) {

                    var toFocus = hit.transform.parent.transform.parent.transform.Find("Focus");
                    if (toFocus == null) {
                        Debug.Log("No focusable object.");
                        return;
                    }
                    Select(toFocus);
                }
            } else {
                SetCursorNormal();
            }
        }
    }

    private void Select(Transform targetTransform) {
        currentlyFocusedBuilding = targetTransform.parent;
        CameraManager.Instance.SwitchCameraTarget(targetTransform);
        UIManager.Instance.UpdateMapUI();
    }

    public void SelectFromButton(Transform targetTransform) {
        var focusableTransform = targetTransform.Find("Focus");
        
        if (focusableTransform == null) {
            Debug.LogError("After button clicked, no focusable transform detected.");
            return;
        }

        currentlyFocusedBuilding = targetTransform;
        CameraManager.Instance.SwitchCameraTarget(targetTransform);
        UIManager.Instance.UpdateMapUI();
    }

    public void SetCursorSelectable() {
        Cursor.SetCursor(selectableCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetCursorNormal() {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    private void HandleInputs() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchMapView();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchTopView();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchFocusedView();
        }

        if (Input.GetMouseButtonDown(0)) {
            selectButtonPressed = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            selectButtonPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            UIManager.Instance.InitQuickSearch();
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

