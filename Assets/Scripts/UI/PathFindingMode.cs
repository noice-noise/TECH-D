using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathFindingMode : MonoBehaviour {

    public GameObject navMeshAgentObject;
    public GameObject pathFindingIndicator;

    private AgentController navMeshAgent;
    private Transform navMeshTargetFollow;

    public Button aerialFollowToggle;
    public Button focusedFollowToggle;
    public Button movementToggle;
    public Button showPathToggle;
    public Button showMarkersToggle;

    private CameraManager.CameraState targetCameraState;
    private CinemachineVirtualCamera followCamera;
    private bool followModeActive;
    
    private bool onAerialFollow;
    private bool onFocusedFollow;

    [SerializeField] private bool canMove;
    [SerializeField] private bool showPath;
    [SerializeField] private bool showMarkers;

    private TextMeshProUGUI aerialFollowText;
    private TextMeshProUGUI focusedFollowText;
    private TextMeshProUGUI movementToggleText;

    public bool onPathFindingMode = false;
    private bool onCoroutineCountdown;
    [SerializeField] float restoreFocusDuration = 1.5f;

    private void Awake() {
        InitPathFindingMode();
    }

    private void InitPathFindingMode() {
        InitNavmeshAgent();
        InitButtonAndListeners();

        followCamera = CameraManager.Instance.topViewCamera;
        pathFindingIndicator.SetActive(false);
    }

    private void InitNavmeshAgent() {
        // initialize navmesh agent and related components
        if (navMeshAgentObject == null) {
            navMeshAgentObject = GameObject.FindGameObjectWithTag("Agent");
        }

        navMeshTargetFollow = navMeshAgentObject.transform.Find("Follow");
        navMeshAgent = navMeshAgentObject.GetComponent<AgentController>();
    }

    private void InitButtonAndListeners() {
        if (aerialFollowToggle != null) {
            aerialFollowToggle.onClick.AddListener(delegate { ToggleAerialFollow(); });
            aerialFollowText = aerialFollowToggle.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        if (focusedFollowToggle != null) {
            focusedFollowToggle.onClick.AddListener(delegate { ToggleFocusedFollow(); });
            focusedFollowText = focusedFollowToggle.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }

        if (movementToggle != null) {
            movementToggle.onClick.AddListener(delegate { ToggleMovement(); });
            movementToggleText = movementToggle.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update() {
        if (onPathFindingMode) {
            HandleFocusRestoration();
            HandleAgentMode();
            HandleCameraReset();
        }
    }

    private void HandleFocusRestoration() {
        bool isAgentFocusTarget = followCamera.m_Follow == navMeshAgent.transform;

        if (followModeActive && !isAgentFocusTarget) {
            if (!onCoroutineCountdown) {
                StartCoroutine(RestoreFollowState(restoreFocusDuration));
            }
        }
    }

    public void StartPathFindingMode() {
        onPathFindingMode = true;
        navMeshAgentObject.SetActive(true);
        pathFindingIndicator.SetActive(true);
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.TopView);
        AgentController.Instance.StartAgentBehavior();
        HandleAerialFollow();
    }

    public void StopPathFindingMode() {
        onPathFindingMode = false;
        AgentController.Instance.StopAgentBehavior();
        pathFindingIndicator.SetActive(false);
    }
    
    public void HandlePathFindingMode() {

    }

    private void HandleAerialFollow() {
        if (onAerialFollow) {
            followModeActive = true;
            followCamera = CameraManager.Instance.topViewCamera;
            targetCameraState = CameraManager.CameraState.TopView;
            CameraManager.Instance.autoSwitchCameraMode = true;
            CameraManager.Instance.SwitchCameraMode(targetCameraState);
            CameraManager.Instance.SwitchTopViewTarget(navMeshTargetFollow);
            aerialFollowText.text = "Aerial Following";
        } else {
            aerialFollowText.text = "Aerial Follow";
            followModeActive = false;
        }
    }

    private void HandleFocusFollow() {
        if (onFocusedFollow) {
            followModeActive = true;
            followCamera = CameraManager.Instance.focusedViewCamera;
            targetCameraState = CameraManager.CameraState.FocusedView;
            CameraManager.Instance.autoSwitchCameraMode = true;
            CameraManager.Instance.SwitchCameraMode(targetCameraState);
            CameraManager.Instance.SwitchFocusedViewTarget(navMeshTargetFollow);
            focusedFollowText.text = "Zoom Following";
        } else {
            focusedFollowText.text = "Zoom Follow";
            followModeActive = false;
        }
    }

    private IEnumerator RestoreFollowState(float time) {
        // while waiting
        onCoroutineCountdown = true;
        yield return new WaitForSeconds(time);
        // after waiting
        CameraManager.Instance.SwitchCameraMode(targetCameraState);
        HandleFollowCameraTargetSwitch();
        onCoroutineCountdown = false;
    }

    private void HandleFollowCameraTargetSwitch() {

        if (followCamera == null) {
            return;
        }

        if (followCamera.Equals(CameraManager.Instance.mapViewCamera)) {
            CameraManager.Instance.SwitchVirtualCameraTarget(0, navMeshTargetFollow);
        } else if (followCamera.Equals(CameraManager.Instance.topViewCamera)) {
            CameraManager.Instance.SwitchVirtualCameraTarget(1, navMeshTargetFollow);
        } else if (followCamera.Equals(CameraManager.Instance.focusedViewCamera)) {
            CameraManager.Instance.SwitchVirtualCameraTarget(2, navMeshTargetFollow);
        } else {
            Debug.LogError("Camera invalid.");
        }
    }

    private void ToggleMovement() {
        canMove = !canMove;
        navMeshAgent.canMove = canMove;
        
        if (canMove) {
            movementToggleText.text = "Lock";
        } else {
            movementToggleText.text = "Move";
        }
    }



    private void HandleAgentMode() {
        if (followModeActive) {
            if (onAerialFollow) {
                focusedFollowToggle.interactable = false;
            }  else if (onFocusedFollow) {
                aerialFollowToggle.interactable = false;
            }
        } else {
            aerialFollowToggle.interactable = true;
            focusedFollowToggle.interactable = true;
        }
    }

    /// <summary>
    /// Only one of either On Aerial and Focused follow must be active, thus they are reversed for every toggle.
    /// </summary>
    private void ToggleAerialFollow() {
        onAerialFollow = !onAerialFollow;
        HandleAerialFollow();
    }
    
    /// <summary>
    /// Only one of either On Aerial and Focused follow must be active, thus they are reversed for every toggle.
    /// </summary>
    private void ToggleFocusedFollow() {
        onFocusedFollow = !onFocusedFollow;
        HandleFocusFollow();
    }

    private void HandleCameraReset() {
        if (!followModeActive) {
            CameraManager.Instance.autoSwitchCameraMode = false;
            CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.MapView);
        }
    }
}
