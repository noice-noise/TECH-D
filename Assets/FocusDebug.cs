using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusDebug : MonoBehaviour
{
    public bool debug;
    public float lineLength;

    private void OnDrawGizmos() {
        if (debug) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, 0, lineLength));
        }
    }
}
