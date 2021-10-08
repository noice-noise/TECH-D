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
    private Transform currentlySelected;

    public bool onTourMode { set; get; } = false;
    public bool autoSwitch = true;

    [SerializeField] private float duration = 5f;


    private bool onCountdown = false;
    public float countdownTimer = 0f;
    public float countdownPercent = 0f;

    public KeyCode tourModeHotKey = KeyCode.F9;
    public KeyCode nextTourModeHotKey = KeyCode.RightArrow;
    public KeyCode prevTourModeHotKey = KeyCode.LeftArrow;

    private void Awake() {
        UpdateTourList();
    }

    public void UpdateTourList() {
        world = GameObject.FindGameObjectWithTag("World");
        tourList = new List<Transform>();
        PopulateTourList();
    }

    private void Update() {
        HandleAllInputs();
        HandleTourAutoSwitch();

        if (onCountdown && countdownTimer > 0) {
            countdownTimer -= Time.deltaTime;
            countdownPercent = countdownTimer / duration;
        } else {
            countdownTimer = 0f;
        }
    }

    public void HandleTourModeToggle() {
        ToggleTourMode();
        HandleTourModeState();
    }

    private void ToggleTourMode() {
        onTourMode = !onTourMode;
    }

    private void OnTourModeKeyDown() {
        if (Input.GetKeyDown(tourModeHotKey)) {
            HandleTourModeToggle();
        }
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

    private void HandleAllInputs() {
        OnTourModeKeyDown();
        HandleTourHotkeys();
    }

    private void HandleTourHotkeys() {
        if (onTourMode) {
            if (Input.GetKeyDown(prevTourModeHotKey)) {
                PreviousTour();
            } else if (Input.GetKeyDown(nextTourModeHotKey)) {
                NextTour();
            }
        }
    }

    public void NextTour() {
        IncrementCounter();
        SelectCurrentTourTarget();
    }

    public void PreviousTour() {
        DecrementCounter();
        SelectCurrentTourTarget();
    }

    private void SelectCurrentTourTarget() {
        currentlySelected = tourList[tourCounter];
        Navigation.Instance.SelectAndUpdateUI(currentlySelected);
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