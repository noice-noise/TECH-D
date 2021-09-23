using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TourMode : MonoBehaviour {
    private GameObject world;
    private List<Transform> tourList;

    private int tourCounter;
    private Transform currentlySelected;

    public bool onTourMode = false;
    public float duration;


    private void Awake() {
        world = GameObject.FindGameObjectWithTag("World");
        tourList = new List<Transform>();
        PopulateTourList();
        // DisplayTourList();
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

    private void Update() {
        HandleAllInputs();
    }

    private void HandleAllInputs() {
        HandleTourModeToggle();
        
        if (onTourMode) {
            HandleTourHotkeys();
        }
    }

    private void HandleTourModeToggle() {
        if (Input.GetKeyDown(KeyCode.T)) {
            onTourMode = !onTourMode;
            Debug.Log("TourMode:l " + onTourMode);
        }
    }

    private void HandleTourHotkeys() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (tourCounter > 1) {
                tourCounter--;
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (tourCounter < tourList.Count() - 1) {
                
                tourCounter++;
            }
        }

        Navigation.Instance.SelectAndUpdateUI(tourList[tourCounter]);
    }

    private void DisplayTourList() {
        foreach (var item in tourList) {
            Debug.Log(item.name);
        }
    }
}
