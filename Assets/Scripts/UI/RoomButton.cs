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

    public void InitRoomButton() {
        



        if (buildingReference == null) {
            GetUIText();

            string titleCaseBuildingName = ToTitleCase();

            Transform targetBuilding = GameObject.Find(titleCaseBuildingName).transform;
            
            if (targetBuilding == null) {
                Building building = FindRoom(uiText);
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

    /// <summary>
    /// Some buttons use native Text or TextMeshPro, thus if no Text detected, look for TextMeshPro.
    /// </summary>
    private void GetUIText() {
        var textTransform = transform.Find("Text");
        var textComponent = transform.Find("Text").GetComponent<Text>();

        if (textComponent != null) {
            uiText = textComponent.text;
        }

        if (textComponent == null || uiText.Trim().Length == 0) {
            uiText = textTransform.GetComponent<TextMeshProUGUI>().text;
        }
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
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uiText.ToLower());
    }

    private void OnClick() {
        UIManager.Instance.HandleButtonClick(transform);
    }
}
