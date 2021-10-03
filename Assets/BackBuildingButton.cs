using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackBuildingButton : MonoBehaviour {
    private Button button;
    public Transform current;

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener( delegate { OnBackButtonClicked(); } );
    }

    private void Update() {
        current = UIManager.Instance.currentlySelectedBuilding;
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
