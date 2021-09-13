using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour {
    public Transform buildingReference;
    private Text uiText;

    private void Start() {
        if (buildingReference == null) {

            uiText = transform.Find("Text").GetComponent<Text>();

            // first letter of needs to be upper because UI.Extensions.AutoCompleteBox displays options in all lowercase for some reason, thus our code needs to adjust
            string capitalizedBuildingName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(uiText.text.ToLower());

            // Transform targetBuilding = UIManager.Instance.world.transform.Find(capitalizedBuildingName);
            Debug.Log("To find: " + capitalizedBuildingName);
            Transform targetBuilding = GameObject.Find(capitalizedBuildingName).transform;
            
            // Debug.Log(targetBuilding.name);

            if (targetBuilding == null) {
                Debug.Log("NULL!!");
                Building building = FindRoom(uiText.text);
                targetBuilding = GameObject.Find(building.buildingName).transform;
                Debug.Log(targetBuilding.name);
                // targetBuilding = GameObject.Find(building.buildingName).transform.Find(uiText.text);
            }

            if (targetBuilding != null) {
                Debug.Log("Reference: " + targetBuilding.name);
                buildingReference = targetBuilding;
            } else {
                Debug.LogError("Building reference not found.");
            }
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

    public void SwitchCameraTarget() {
        Navigation.Instance.SelectFromButton(buildingReference);
    }
}
