using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackBuildingButton : MonoBehaviour {
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Update() {
        Debug.Log("Update?");
        var current = UIManager.Instance.currentlySelectedBuilding;

        var parent = current.parent.transform.parent;



        if (current != null && parent != null && parent.CompareTag("SelectableBuilding")) {
            button.interactable = true;
        } else {
            button.interactable = false;
        }
    }
}
