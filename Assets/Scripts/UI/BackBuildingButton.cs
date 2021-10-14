using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackBuildingButton : MonoBehaviour {
    private Button button;
    private Transform current;
    public KeyCode backHotKey = KeyCode.Escape;


    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener( delegate { OnBackButtonClicked(); } );
    }

    private void Update() {
        current = UIManager.Instance.currentlySelectedBuilding;
        HandleHotKey();
    }

    private void HandleHotKey() {
        if (Input.GetKeyDown(backHotKey)) {
            button.onClick.Invoke();
        }
    }

    public void OnBackButtonClicked() {
        if (current == null ) {
            return;
        }

        var parent = current.parent.transform.parent;
        if (parent != null && parent.CompareTag("SelectableBuilding")) {
            Navigation.Instance.SelectAndUpdateUI(parent);
        } else {
            UIManager.Instance.HandleBuildingPane();
        } 
    }
}
