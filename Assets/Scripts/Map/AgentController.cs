using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class AgentController : Singleton<AgentController> {

    public NavMeshAgent agent;
    private Transform target;

    public LineRenderer lineRenderer;
    [SerializeField] private float lineHeightOffset = 2f;

    private void Start() {

        lineRenderer.positionCount = 0;

        CameraManager.OnCameraTargetChanged += TargetChanged;
    }

    private void OnDisable() {
        CameraManager.OnCameraTargetChanged -= TargetChanged;
    }

    private void TargetChanged() {
        target = CameraManager.Instance.currentTarget;
        Debug.Log("Target Changed: " + target.parent.name);
        SetAgentDestination(target.position);
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
}
