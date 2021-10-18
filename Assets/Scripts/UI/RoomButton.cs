using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour {

    public Button button;
    public Transform buildingReference;
    private string uiText;

    private void Start() {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        InitRoomButton();
    }

    private void InitRoomButton() {

        if (buildingReference == null) {
            CheckUIText();
            GetBuildingReference(uiText);
        }

        HandleSelectableBuildingTag();
    }

    private void GetBuildingReference(string titleCaseBuildingName) {
        Transform targetBuilding = GameObject.Find(titleCaseBuildingName).transform;
        
        if (targetBuilding == null) {
            targetBuilding = FindTarget();
        }

        if (targetBuilding != null) {
            // Debug.Log("Reference: " + targetBuilding.name);
            buildingReference = targetBuilding;
        } else {
            Debug.LogError("Building reference not found.");
        }
    }

    public Transform FindTarget() {
        Building building = FindTransform(uiText);
        return GameObject.Find(building.buildingName).transform;
    }

    /// <summary>
    /// Check for UI type Text otherwise check for TextMeshPro
    /// </summary>
    private void CheckUIText() {
        var textTransform = transform.Find("Text");
        var textComponent = transform.Find("Text").GetComponent<Text>();

        if (textComponent != null) {
            uiText = textComponent.text;
        }

        if (textComponent == null || uiText.Trim().Length == 0) {
            uiText = textTransform.GetComponent<TextMeshProUGUI>().text;
        }

        uiText = ToTitleCase(uiText);
    }

    // If the current transform reference is the primary Building transform with "SelectableTag"
    // change it to Focus as it ensures clean FocusedView
    private void HandleSelectableBuildingTag() {
        if (buildingReference.CompareTag("SelectableBuilding")) {
            buildingReference = buildingReference.Find("Focus");
        }
    }

    private Building FindTransform(string targetRoomText) {
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
    private string ToTitleCase(string text) {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
    }

    private void OnClick() {
        UIManager.Instance.HandleButtonClick(transform);
    }

    public void UpdateRoomReferenceWith(string referenceName) {
        GetBuildingReference(referenceName);
    }
}
