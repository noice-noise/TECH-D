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

    public ParticleSystem dust;

    public Transform originMark;
    public Transform followMark;
    public Transform targetMark;

    public bool canMove { get; set; } = false;
    public bool enableMarkers{ get; set; }  = false;
    public bool showDrawPath { get; set; }  = false;
    public bool canDrawPath { get; set; }  = true;

    private void Start() {
        lineRenderer.positionCount = 0;
        agent.isStopped = true;
        CameraManager.OnCameraTargetChanged += OnTargetChanged;

        followMark.SetParent(transform);
        SetOriginPosition(transform.position);
    }

    private void OnDisable() {
        CameraManager.OnCameraTargetChanged -= OnTargetChanged;
    }

    private void Update() {
        if (showDrawPath) {
            HandleDrawPath();
        } else {
            lineRenderer.enabled = false;
        }

        if (enableMarkers) {
            SetAllMarkerActiveSelf(true);
        } else {
            SetAllMarkerActiveSelf(false);
        }

        if (canMove && agent.hasPath) {
            agent.isStopped = false;
        } else {
            agent.isStopped = true;
        }
    }

    private void SetAllMarkerActiveSelf(bool state) {
        originMark.gameObject.SetActive(state);
        targetMark.gameObject.SetActive(state);
        followMark.gameObject.SetActive(state);
    }

    private void HandleDrawPath() {
        if (canDrawPath && agent.hasPath) {
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
        HandleAgentBehavior();
    }

    private void HandleAgentBehavior() {
        UpdateTarget();
        SetDestination();
        UpdateTargetMark();
    }

    private void SetDestination() {
        agent.SetDestination(target.position);
    }

    private void UpdateTarget() {
        target = CameraManager.Instance.currentTarget;

        if (target.CompareTag("FocusedViewTarget")) {
            target = target.parent;
        }
    }

    private void UpdateTargetMark() {
        SetTargetPosition(target.position);
    }

    private void HandleEffects() {
        if (agent.transform.hasChanged) {
            dust.Play();
        } else {
            dust.Stop();
        }
    }

    public void StartAgentBehavior() {
        enableMarkers = true;
        showDrawPath = true;
        SetAllMarkerActiveSelf(true);
        
        HandleEffects();
    }

    public void StopAgentBehavior() {
        enableMarkers = false;
        showDrawPath = false;
        SetAllMarkerActiveSelf(false);
        DisableAgentMovement();
        HandleEffects();
    }

    private void EnableAgentMovement() {
        agent.isStopped = true;
    }

    private void DisableAgentMovement() {
        agent.isStopped = true;
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
