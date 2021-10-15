using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimator : MonoBehaviour {

    // TODO Disallow changes in line origin and target while animating
    public bool allowAnimator = true;
    // private bool isAnimating = false;

    private Vector3[] linePoints;
    private int pointsCount;

    [SerializeField] private float animationDelay = 1f;
    [SerializeField] private float animationDuration = 2f;
    private LineRenderer lineRenderer;
    
    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {

    }

    public void StartAnimationWithDelay() {
        StartCoroutine(DelayAction(animationDelay));
    }

    private IEnumerator DelayAction(float timeDelay){
        yield return new WaitForSeconds(timeDelay);
        StartAnimation();
    }

    public void StartAnimation() {
        PrepareLineCorners();
        StartCoroutine(AnimateLine());
    }

    private void PrepareLineCorners() {
        pointsCount = lineRenderer.positionCount;
        linePoints = new Vector3[pointsCount];

        for (int i = 0; i < pointsCount; i++) {
            linePoints[i] = lineRenderer.GetPosition(i);
        }
    }

    private IEnumerator AnimateLine() {
        // isAnimating = true;
        float segmentDuration = animationDuration / pointsCount;

        for (int i = 0; i < pointsCount - 1; i++) {
            float startTime = Time.time;

            Vector3 startPosition = linePoints[i];
            Vector3 endPosition = linePoints[i + 1];

            Vector3 pos = startPosition;

            while (pos != endPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                for (int j = i + 1; j < pointsCount; j++) {
                    lineRenderer.SetPosition(j, pos);
                }

                yield return null;
            }
        }

        // isAnimating = false;
    }
}
