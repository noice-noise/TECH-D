using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TourModeButton : MonoBehaviour{

    public GameObject tourModeObject;
    private TourMode tourMode;
    [SerializeField] private bool onTourMode;

    private Button button;
    private ColorBlock defaultColor;

    private void Awake() {
        tourMode =  tourModeObject.GetComponent<TourMode>();
        button = GetComponent<Button>();
        defaultColor = button.colors;
    }

    private void Update() {
        onTourMode = tourMode.onTourMode;
        HandleButtonState();
    }

    private void HandleButtonState() {
        if (onTourMode) {
            ChangeNormalColor();
        } else {
            RestoreColor();
        }
    }

    private void RestoreColor() {
        button.colors = defaultColor; 
    }

    private void ChangeNormalColor() {
        ColorBlock colorVal =  button.colors;
        colorVal.normalColor = colorVal.highlightedColor;
        button.colors = colorVal;
    }
} 
