using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class TourMode : Singleton<TourMode>
{
    private GameObject world;
    private List<Transform> tourList;

    private int tourCounter;
    // private Transform currentlySelected;

    public bool onTourMode { set; get; } = false;
    public bool autoSwitch = true;

    [Tooltip("Duration of auto switch")]
    [SerializeField] private float idleDuration = 5f;   // duration while on normal cycle
    [Tooltip("Duration of auto switch if user tries to visit specific Services/Rooms")]
    [SerializeField] private float cycleDuration = 10f;

    private float duration;
    private bool onCountdown = false;
    public float countdownTimer = 0f;
    public float countdownPercent = 0f;

    public KeyCode nextTourModeHotKey = KeyCode.RightArrow;
    public KeyCode prevTourModeHotKey = KeyCode.LeftArrow;

    private void Awake() {
        InitTourList();
        duration = idleDuration;

        CameraManager.OnCameraTargetChanged += HandleTargetChange;
    }

    private void HandleTargetChange() {
        if (onTourMode) {
            StopNextTourTargetCoroutine();
            HandleAutoSwitchDuration();
            HandleTourAutoSwitch();
        }
    }

    public void InitTourList() {
        world = GameObject.FindGameObjectWithTag("World");
        tourList = new List<Transform>();
        PopulateTourList();
    }

    private void Update() {
        if (onTourMode) {
            HandleTourAutoSwitch();

            if (onCountdown && countdownTimer > 0) {
                countdownTimer -= Time.deltaTime;
                countdownPercent = countdownTimer / duration;
            } else {
                countdownTimer = 0f;
            }
        }
    }

    public void HandleTourModeToggle() {
        ToggleTourMode();
        HandleTourModeState();
    }

    private void ToggleTourMode() {
        onTourMode = !onTourMode;
    }

    private void HandleTourModeState() {
        if (!onTourMode) {
            StopNextTourTargetCoroutine();
        } else if (onTourMode) {
            SelectCurrentTourTarget();
        }
    }

    private void HandleTourAutoSwitch() {
        if (onTourMode && autoSwitch && !onCountdown) {
            countdownTimer = duration;
            StartCoroutine("NextTourTargetCoroutine");
        }
    }

    private void HandleAutoSwitchDuration() {
        bool notAFocusableBuilding = !CameraManager.Instance.currentTarget.CompareTag("FocusedViewTarget");
        if (notAFocusableBuilding) {
            duration = cycleDuration;
        } else {
            duration = idleDuration;
        }
    }

    IEnumerator NextTourTargetCoroutine() {
        onCountdown = true;
        yield return new WaitForSeconds(duration);
        IncrementCounter();
        SelectCurrentTourTarget();
        onCountdown = false;
    }

    private void StopNextTourTargetCoroutine() {
        onCountdown = false;
        StopCoroutine("NextTourTargetCoroutine");
    }

    public void NextTour() {
        UpdateCurrentTarget();
        StopNextTourTargetCoroutine();
        IncrementCounter();
        SelectCurrentTourTarget();
        HandleTourAutoSwitch();
    }

    public void PreviousTour() {
        UpdateCurrentTarget();
        StopNextTourTargetCoroutine();
        DecrementCounter();
        SelectCurrentTourTarget();
        HandleTourAutoSwitch();
    }

    private void UpdateCurrentTarget() {
        Debug.Log("Update");
        var targetTransform = CameraManager.Instance.currentTarget;
        var currentTourTarget = tourList[tourCounter];
        
        if (targetTransform.CompareTag("FocusedViewTarget")) {
            targetTransform = targetTransform.parent;
        }

        if (!currentTourTarget.Equals(targetTransform)) {
            int index = FindIndexOf(targetTransform);

            if (index != -1) {
                tourCounter = index;
            }
        }
    }

    private int FindIndexOf(Transform targetTransform) {
        Debug.Log("Target: " + targetTransform.name);
        for (int i = 0; i < tourList.Count; i++) {
            if (tourList[i].Equals(targetTransform)) {
                Debug.Log("Success");
                return i;
            }
        }

        return -1;  // invalid
    }

    private void SelectCurrentTourTarget() {
        Navigation.Instance.SelectAndUpdateUI(tourList[tourCounter]);
    }
    

    /// <summary>
    /// Populates the tour list based on the order of GameObjects on "World"
    /// </summary>
    private void PopulateTourList() {
        var list = world.transform.Cast<Transform>().ToList();

        foreach (Transform item in list) {
            if (item != null && item.CompareTag("SelectableBuilding")) {
                tourList.Add(item);
            }
        }
    }

    private void IncrementCounter() {
        int tourListLength = tourList.Count();

        if (tourCounter < tourListLength) {
            tourCounter++;
        } 

        if (tourCounter >= tourListLength) {
            ResetCounter();
        }
    }

    private void ResetCounter() {
        tourCounter = 0;
    }

    private void DecrementCounter() {
        if (tourCounter > 0) {
            tourCounter--;
        }

        if (tourCounter <= 0) {
            MaxCounter();
        }
    }

    private void MaxCounter() {
        tourCounter = tourList.Count() - 1;
    }

    private void DisplayTourList() {
        foreach (var item in tourList) {
            Debug.Log(item.name);
        }
    }
}