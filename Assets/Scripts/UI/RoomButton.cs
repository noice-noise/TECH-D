using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour {

    public Button button;
    public Transform buildingReference;
    private Text uiText;

    private void Start() {

        button = this.GetComponent<Button>();
        button.onClick.AddListener(OnClick);


        if (buildingReference == null) {
            uiText = transform.Find("Text").GetComponent<Text>();

            string titleCaseBuildingName = ToTitleCase();

            Transform targetBuilding = GameObject.Find(titleCaseBuildingName).transform;
            
            if (targetBuilding == null) {
                Building building = FindRoom(uiText.text);
                targetBuilding = GameObject.Find(building.buildingName).transform;
            }

            if (targetBuilding != null) {
                Debug.Log("Reference: " + targetBuilding.name);
                buildingReference = targetBuilding;
            } else {
                Debug.LogError("Building reference not found.");
            }
        }

        HandleSelectableBuildingTag();
    }

    // If the current transform reference is the primary Building transform with "SelectableTag"
    // change it to Focus as it ensures clean FocusedView
    private void HandleSelectableBuildingTag() {
        if (buildingReference.CompareTag("SelectableBuilding")) {
            buildingReference = buildingReference.Find("Focus");
        }
    }

    private Building FindRoom(string targetRoomText) {
        foreach (var building in UIManager.Instance.mapData.buildingData) {
            foreach (var room in building.roomChildren) {
                if (room.roomName.Equals(targetRoomText)) {
                    return building;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// first letter of needs to be upper because UI.Extensions.AutoCompleteBox displays options in all lowercase for some reason, thus our code needs to adjust
    /// </summary>
    private string ToTitleCase() {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uiText.text.ToLower());
    }

    private void OnClick() {
        UIManager.Instance.HandleButtonClick(transform);
    }
}
