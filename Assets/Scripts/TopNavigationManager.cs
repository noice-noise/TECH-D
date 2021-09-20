using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopNavigationManager : MonoBehaviour {

    public Transform leftNav;
    public Transform queriesPanel;

    public TopNavButtons navButton;

    public Transform currentlyActivePanel;  //TODO implement currently active panel

    private void Awake() {
        InitListeners(navButton.Home);
        InitListeners(navButton.Map);
        InitListeners(navButton.Queries);
    }

    private void InitListeners(GameObject navButton) {
        navButton.GetComponent<Button>().onClick.AddListener( 
            delegate { SelectionHandler(navButton); });
    }

    public void SelectionHandler(GameObject button) {
        Debug.Log("Click: ");

        if (button != null) {

            if (button == navButton.Home) {
                Debug.Log("Home!");
            } else if (button == navButton.Map) {
                Debug.Log("MAP!");
            } else if (button == navButton.Queries) {
                Debug.Log("QUERIES!");
            }
        }
    }
}

[System.Serializable]
public class TopNavButtons {
    public GameObject Home;
    public GameObject Map;
    public GameObject Queries;
} 
