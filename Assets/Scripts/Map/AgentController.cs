using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class AgentController : Singleton<AgentController> {

    public NavMeshAgent agent;
    private Transform target;

    public LineRenderer lineRenderer;
    [SerializeField] private float lineHeightOffset = 2f;

    public Transform originMark;
    public Transform followMark;
    public Transform targetMark;

    [SerializeField] private bool canMove = false;
    [SerializeField] private bool enableMarkers = false;
    [SerializeField] private bool canDrawPath = false;

    private void Start() {
        lineRenderer.positionCount = 0;
        CameraManager.OnCameraTargetChanged += OnTargetChanged;

        followMark.SetParent(transform);
        SetOriginPosition(transform.position);
    }

    private void OnDisable() {
        CameraManager.OnCameraTargetChanged -= OnTargetChanged;
    }

    public void EnableMovement() {
        canMove = true;
    }

    public void DisableMovement() {
        canMove = false;
    }

    private void Update() {
        if (canDrawPath) {
            HandleDrawPath();
        } else {
            lineRenderer.enabled = false;
        }

        if (enableMarkers) {
            SetAllMarkerActiveSelf(true);
        } else {
            SetAllMarkerActiveSelf(false);
        }
    }

    private void SetAllMarkerActiveSelf(bool state) {
        originMark.gameObject.SetActive(state);
        targetMark.gameObject.SetActive(state);
        followMark.gameObject.SetActive(state);
    }

    private void HandleDrawPath() {
        if (agent.hasPath) {
            lineRenderer.enabled = true;
            DrawPath();
        }
    }



    public void DrawPath() {
        Vector3[] pathCorners = agent.path.corners;

        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPosition(0, transform.position);

        if (pathCorners.Length < 2) {
            return;
        }

        for (int i = 0; i < pathCorners.Length; i++) {

            Vector3 pointPosition = new Vector3(pathCorners[i].x, pathCorners[i].y + lineHeightOffset, pathCorners[i].z);

            lineRenderer.SetPosition(i, pointPosition);
        }
    }

    private void OnTargetChanged() {
        target = CameraManager.Instance.currentTarget;
        if (target.CompareTag("FocusedViewTarget")) {
            target = target.parent;
        }
        Debug.Log("Target Changed: " + target.parent.name);
        
        if (canMove) {
            SetAgentDestination(target.position);
        }

        HandleTargetMark();
    }

    private void HandleTargetMark() {
        SetTargetPosition(target.position);
    }

    public void StartAgentBehavior() {
        canMove = true;
        enableMarkers = true;
        canDrawPath = true;
        SetAllMarkerActiveSelf(true);
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    public void StopAgentBehavior() {
        canMove = false;
        enableMarkers = false;
        canDrawPath = false;
        SetAllMarkerActiveSelf(false);
        agent.isStopped = true;
    }

    public void SetAgentDestination(Vector3 target) {
        agent.SetDestination(target);
    }

    private void SetOriginPosition(Vector3 position) {
        originMark.position = position;
    }

    private void SetTargetPosition(Vector3 position) {
        targetMark.position = position;
    }

    private void SetFollowPosition(Vector3 position) {
        followMark.position = position;
    }
}
