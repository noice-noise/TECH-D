
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI.Extensions;
using System;

public class UIManager : Singleton<UIManager> {
    
    [Header("Hierarchy")]
    public GameObject mainCanvas;
    public GameObject world;
    public GameObject help;

    [Header("UI")]
    public Transform leftNav;
    public Transform leftNavTitle;
    public Transform leftNavDescription;

    public Transform buildingContainer;
    public Transform listInServicesList;
    public Transform listInRoomsList;

    public Transform searchContainer;

    public GameObject cameraModeDropdown;

    [Header("Instantiation")]
    public GameObject roomsButtonPrefab;
    public GameObject quickSearchPrefab;

    public Transform currentlySelectedBuilding;
    private IEnumerable<Transform> worldChildren;

    public MapData mapData;

    private void Start() {
        worldChildren = world.transform.Cast<Transform>();
        HandleBuildingPane();
        InitQuickSearch();
    }

    public void UpdateMapUI() {
        currentlySelectedBuilding = Navigation.Instance.currentlyFocusedBuilding;
        UpdateHeader();
        UpdateBody();
    }

    /// <summary>
    /// Update UI Header Title and Description based on MapData
    /// </summary>
    private void UpdateHeader() {
        var title = leftNavTitle.GetComponent<TextMeshProUGUI>();
        string buildingName = currentlySelectedBuilding.transform.name;
        title.SetText(buildingName);

        var description = leftNavDescription.GetComponent<TextMeshProUGUI>();
        bool descriptionAvailable = false;

        // Get information from MapData
        foreach (var item in mapData.buildingData) {
            if (item.buildingName.Trim().Equals(buildingName.Trim())) {
                description.SetText(item.description);
                descriptionAvailable = true;
            }
        }

        // Set default description if there's no available data
        if (!descriptionAvailable) {
            bool isSelectableBuilding = currentlySelectedBuilding.CompareTag("SelectableBuilding");
            
            if (isSelectableBuilding) {
                description.SetText("No detailed information yet. Further information about this building will be updated soon.");
            }

            // Set description to empty if parent is null, then TERMINATE function
            if (currentlySelectedBuilding.parent == null) {
                description.SetText("");
                return;
            }

            // Set default description based on type
            bool isServiceType = currentlySelectedBuilding.parent.name.Equals("FocusableServices");
            bool isRoomType = currentlySelectedBuilding.parent != null && currentlySelectedBuilding.parent.name.Equals("FocusableRooms");

            if (isServiceType) {
                description.SetText("A building service to support Teknoys with their day to day student life. Further information about the service will be updated soon.");
            } else if (isRoomType) {
                description.SetText("A lecture room where Teknoys learn to be good their chosen crafts. Further information about the lecture room will be updated soon.");
            }
        }
    }

    /// <summary>
    /// Update the UI Body which contains Services or Rooms sections and buttons
    /// </summary>
    private void UpdateBody() {

        SetListParentActive(listInServicesList, true);
        SetListParentActive(listInRoomsList, true);


        listInServicesList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Services");

        listInRoomsList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Rooms");

        UpdatePanelContents(listInServicesList, "FocusableServices");
        UpdatePanelContents(listInRoomsList, "FocusableRooms");

    }

    private void UpdatePanelContents(Transform list, string transformName) {

        var focusTransform = currentlySelectedBuilding.Find(transformName);

        if (focusTransform == null) {
            SetListParentActive(list, false);
            return;
        }

        List<Transform> focusableServiceRooms = focusTransform.Cast<Transform>().ToList();

        if (focusableServiceRooms != null && focusableServiceRooms.Count == 0) {
            SetListParentActive(list, false);
            return;
        }
        
        ClearChildren(list);

        for (int i = 0; i < focusableServiceRooms.Count; i++) {
            CreateButton(focusableServiceRooms[i], list, roomsButtonPrefab);
        }
    }

    private void SetListParentActive(Transform list, bool parentState) {
        var obj = list.parent.parent.gameObject;
        obj.SetActive(parentState);
    }

    private void CreateButton(Transform transformSource, Transform targetParent, GameObject buttonPrefab) {

        GameObject newButton = Instantiate(buttonPrefab, targetParent, false);
        newButton.transform.SetParent(targetParent);
        newButton.transform
            .Find("Text")
            .GetComponent<TextMeshProUGUI>()
            .SetText(transformSource.transform.name);
        newButton.GetComponent<RoomButton>().buildingReference = transformSource;
    }

    private void ClearChildren(Transform uiContents) {
        if (uiContents.transform.childCount == 0) {
            return;
        }

        foreach (Transform child in uiContents.transform) {
            Destroy(child.gameObject);
        }
    }

    public void HandleButtonClick(Transform button) {

        if (button.name.Equals("Building")) {

            HandleBuildingPane();
            ToggleSearchVisibility(buildingContainer, true);
            ToggleSearchVisibility(searchContainer, false);

        } else if (button.name.Equals("Search")) {

            ToggleSearchVisibility(searchContainer, true);
            ToggleSearchVisibility(buildingContainer, false);

        } else {

            var targetTransform = button.GetComponent<RoomButton>().buildingReference;
            Navigation.Instance.SelectAndUpdateUI(targetTransform);
        }
    }

    private void ToggleSearchVisibility(Transform targetTransform, bool targetState) {
        targetTransform.gameObject.SetActive(targetState);
    }

    public void InitQuickSearch() {
        List<string> searchList = new List<string>();

        foreach(Transform gameObject in worldChildren) {
            if (gameObject.CompareTag("SelectableBuilding")) {
                searchList.Add(gameObject.name);

                List<string> focusableServices = GetChildTransforms(gameObject, "FocusableServices");
                searchList.AddRange(focusableServices);

                List<string> focusableRooms = GetChildTransforms(gameObject, "FocusableRooms");
                searchList.AddRange(focusableRooms);
            }
        }

        List<string> availableOptions = quickSearchPrefab.GetComponent<AutoCompleteComboBox>().AvailableOptions = searchList;    
        GameObject search = Instantiate(quickSearchPrefab, searchContainer, false);
    }

    private List<string> GetChildTransforms(Transform source, string focusableStringID) {
        var focusable = source.transform.Find(focusableStringID);
    
        List<string> list = new List<string>();

        if (focusable != null) {
            IEnumerable<Transform> focusableRooms = focusable.Cast<Transform>();
            foreach (Transform room in focusableRooms) {
                list.Add(room.name);
            }
        }

        return list;
    }

    public void HandleBuildingPane() {
        DisplayAllBuildingPane();
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.MapView);

    }

    private void DisplayAllBuildingPane() {
        ClearChildren(listInRoomsList);
        SetListParentActive(listInRoomsList, true);

        ClearChildren(listInServicesList);
        SetListParentActive(listInServicesList, true);

        leftNavTitle.GetComponent<TextMeshProUGUI>().SetText("CIT University");
        leftNavDescription.GetComponentInChildren<TextMeshProUGUI>().SetText("Cebu Institute of Technology University is a private non-sectarian academic institution in Cebu City, Philippines.");

        listInServicesList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Building");

        listInRoomsList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Vicinity");

        foreach(Transform child in worldChildren) {
            if (child.CompareTag("SelectableBuilding")) {
                // a building with FocusableRooms will be considered a university Building 
                // otherwise, it will be considered "Vicinity" or a building outside university premises
                if (child.Find("FocusableRooms") != null) {
                    CreateButton(child, listInServicesList, roomsButtonPrefab);
                } else {
                    CreateButton(child, listInRoomsList, roomsButtonPrefab);
                }
            } 
        }
    }

    public void ToggleHelpPanel() {
        // only show help toggle if on University Map
        if (leftNav.gameObject.activeSelf == true) {
            // lazily show or hide
            help.SetActive(!help.activeSelf);
        }
    }

    public void BackToParentBuilding() {
        if (currentlySelectedBuilding == null) {
            return;
        }

        var parentBuilding = currentlySelectedBuilding.parent.transform.parent;
        if (parentBuilding != null && parentBuilding.CompareTag("SelectableBuilding")) {
            Navigation.Instance.SelectAndUpdateUI(parentBuilding);
        }
    }
}
