using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TourModeProgressBar : MonoBehaviour {

    private Slider slider;

    private void Awake() {
        slider = transform.Find("Progress Bar").GetComponent<Slider>();
    }

    private void Update() {
        if (TourMode.Instance.onTourMode) {
            slider.value = TourMode.Instance.countdownPercent;
        }
    }
}
