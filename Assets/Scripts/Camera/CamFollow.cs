using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CamFollow : MonoBehaviour
{
    private Transform lookAt;
    [SerializeField] public Vector3 offset;
    private TextMeshProUGUI text;
    private Camera cam;

    private void Awake() {
        cam = Camera.main;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        text.SetText(transform.parent.transform.name);
    }

    private void Update() {
        transform.LookAt(cam.transform);
    }
}
