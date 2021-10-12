using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public NavMeshData navMeshData;

    private void Awake () {
        navMeshSurface.BuildNavMesh();
    }
}
