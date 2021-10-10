using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TourModeHeader : MonoBehaviour {

    private TextMeshProUGUI text;

    private void Awake() {
        var textTrans = transform.Find("Text");

        if (textTrans == null) {
            Debug.LogError("TextMeshProUGUI text not set.");
        }

        text = textTrans.GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if (TourMode.Instance.onTourMode) {
            HandleHeaderUpdate();
        }
    }

    private void HandleHeaderUpdate() {
        var target = UIManager.Instance.currentlySelectedBuilding;
        if (target == null) return;
        Debug.Log("NN: " + target.name);
        text.text = target.name;
    }
}
