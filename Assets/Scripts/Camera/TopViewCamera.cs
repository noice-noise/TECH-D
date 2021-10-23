using UnityEngine;
using Cinemachine;
using System;

public class TopViewCamera : MonoBehaviour {

    [Header("On Interactive/TourMode Mode")]
    public Vector3 adjustedFollowOffset;
    public Vector3 adjustedTrackedObjectOffset = Vector3.zero;

    [Header("On Path Finding Mode")]
    [SerializeField] private Vector3 adjustedAgentFollowOffset;
    [SerializeField] private Vector3 adjustedAgentTrackedObjectOffset = Vector3.zero;

    [Header("Original Follow Offset")]
    [SerializeField] private Vector3 baseFollowOffset;
    [SerializeField] private Vector3 baseTrackedObjectOffset;

    private CinemachineVirtualCamera topViewCamera;
    private Transform currentTarget;
    public bool allowOffset = true;

    public MapOrientation mapOrientation;

    private void Awake() {
        topViewCamera = GetComponent<CinemachineVirtualCamera>();
        baseFollowOffset =  topViewCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        baseTrackedObjectOffset =  topViewCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;

        // copy and modify top camera offsets with reversed x and z on adjustedFollow
        adjustedFollowOffset = new Vector3(-1 * baseFollowOffset.x, baseFollowOffset.y, -1 * baseFollowOffset.z);
        adjustedTrackedObjectOffset = new Vector3(baseTrackedObjectOffset.x, baseTrackedObjectOffset.y,baseTrackedObjectOffset.z);

        adjustedAgentFollowOffset = new Vector3(
                -1 * (baseFollowOffset.x - 20), 
                baseFollowOffset.y, 
                -1 * (baseFollowOffset.z - 20));

        // adjustedAgentTrackedObjectOffset = new Vector3(baseTrackedObjectOffset.x, baseTrackedObjectOffset.y,baseTrackedObjectOffset.z);
    }


    private void Update() {
        currentTarget = topViewCamera.m_Follow;

        if (allowOffset) {
            HandleMapOrientation(mapOrientation.Value());
        }
    }

    public void HandleMapOrientation(int value) {
        int southEastFacing = 0;

        if (value == southEastFacing) {
            HandleAllFollowOffsets();
            HandleAllAgentOffsets();
        } else {
            RestoreAllOffsets();
        }

        if (ModeManager.Instance.OnPathFindingMode()) {
            AdjustTrackedOffset(topViewCamera, adjustedAgentTrackedObjectOffset);
            // if (value == southEastFacing) {
            //     HandleAllAgentOffsets();
            // } else {
            //     AdjustTrackedOffset(topViewCamera, adjustedAgentTrackedObjectOffset);
            // }
        }
    }

    private void RestoreAllOffsets() {
        RestoreAimOffset(topViewCamera, baseFollowOffset);
        RestoreTrackedOffset(topViewCamera, baseTrackedObjectOffset);
    }

    private void HandleAllFollowOffsets () {
        AdjustAimOffset(topViewCamera, adjustedFollowOffset);
        AdjustTrackedOffset(topViewCamera, adjustedTrackedObjectOffset);
    }

    private void HandleAllAgentOffsets() {
        AdjustAimOffset(topViewCamera, adjustedAgentFollowOffset);
        AdjustTrackedOffset(topViewCamera, adjustedAgentTrackedObjectOffset);
    }

    private void AdjustAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = offset;
    }

    private void RestoreAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = baseFollowOffset;
    }

    private void AdjustTrackedOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camComposer = camera.GetCinemachineComponent<CinemachineComposer>();
        camComposer.m_TrackedObjectOffset = offset;
    }

    private void RestoreTrackedOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camComposer = camera.GetCinemachineComponent<CinemachineComposer>();
        camComposer.m_TrackedObjectOffset = baseTrackedObjectOffset;
    }
}