using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {

    public NavMeshAgent agent;

    public LineRenderer lineRenderer;

    private void Start() {
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;
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
            Vector3 pointPosition = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointPosition);
        }
    }
}
