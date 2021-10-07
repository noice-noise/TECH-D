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

    private void Start() {
        // lineRenderer.startWidth = 0.15f;
        // lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;
    }

    private void Update() {
        target = CameraManager.Instance.currentTarget;

        if (target.position != null) { 
            SetAgentDestination(target.position);
        }

        if (agent.hasPath) {
            DrawPath();
        }
    }

    public void SetAgentDestination(Vector3 target) {
        // Debug.Log("Request move");
        agent.SetDestination(target);
    }

    public void DrawPath() {
        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPosition(0, transform.position);

        // int corners = agent.path.corners.Length;

        if (agent.path.corners.Length < 2) {
            return;
        }

        for (int i = 0; i < agent.path.corners.Length; i++) {
            Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y+2, agent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }
}
