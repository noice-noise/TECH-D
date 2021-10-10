using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFindingMode : MonoBehaviour {

    public GameObject navMeshAgentObject;
    public GameObject pathFindingIndicator;

    public AgentController navMeshAgent;

    public Button aerialFollow;
    public Button focusedFollow;
    public Button movementToggle;
    public Button showPathToggle;
    public Button showMarkersToggle;

        bool onAerialFollow;
        bool onFocusedFollow;
        bool canMove;
        bool showPath;
        bool showMarkers;

    private void Start() {

        if (navMeshAgentObject == null) {
            navMeshAgentObject = GameObject.FindGameObjectWithTag("Agent");
        }

        pathFindingIndicator.SetActive(false);

        if (aerialFollow != null) {
            aerialFollow.onClick.AddListener(delegate { OnAerialFollow(); });
        }

        if (movementToggle != null) {
            movementToggle.onClick.AddListener(delegate { ToggleMovement(); });
        }
    }

    private void ToggleMovement() {
        canMove = !canMove;
        navMeshAgent.canMove = canMove;
    }

    private void OnAerialFollow() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        CameraManager.Instance.SwitchTopViewTarget(navMeshAgentObject.transform);
    }

    public bool onPathFindingMode = false;


    private void Update() {

    }

    public void StartPathFindingMode() {
        navMeshAgentObject.SetActive(true);
        pathFindingIndicator.SetActive(true);
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        AgentController.Instance.StartAgentBehavior();
    }

    public void StopPathFindingMode() {
        AgentController.Instance.StopAgentBehavior();
        pathFindingIndicator.SetActive(false);
    }

    public void FollowAgent() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        CameraManager.Instance.SwitchTopViewTarget(navMeshAgentObject.transform);
    }

    public void FollowAgentViaFocusedCamera() {
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
        CameraManager.Instance.SwitchFocusedViewTarget(navMeshAgentObject.transform.Find("Follow"));
    }

    public void HandlePathFindingMode() {

    }
}
