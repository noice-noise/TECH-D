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

    private void Start() {
        lineRenderer.positionCount = 0;
        CameraManager.OnCameraTargetChanged += OnTargetChanged;

        followMark.SetParent(transform);
        // followMark.position = new Vector3(0, 0, 0);
        SetOriginPosition(transform.position);
    }

    private void OnDisable() {
        CameraManager.OnCameraTargetChanged -= OnTargetChanged;
    }

    private void Update() {
        if (agent.hasPath) {
            DrawPath();
        }
    }

    public void SetAgentDestination(Vector3 target) {
        // Debug.Log("Request move");
        agent.SetDestination(target);
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
        Debug.Log("Target Changed: " + target.parent.name);
        SetAgentDestination(target.position);
        HandleTargetMark();
    }

    private void HandleTargetMark() {
        // targetMark.transform.SetParent(target);
        SetTargetPosition(target.position);
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
