using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.UI.Extensions;
using DG.Tweening;

public class UIManager : Singleton<UIManager> {
    
    [Header("Hierarchy")]
    public GameObject mainCanvas;
    public GameObject world;

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

    public Transform currentlyActivePanel;  // TODO: delegate to NAV
    public Transform currentlySelectedBuilding;
    private IEnumerable<Transform> worldChildren;

    public MapData mapData;
    public bool uIFullscreen = false;

    [Header("Tweening")]

    public Ease ease;
    public float duration;
    public float distance;

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

    private void UpdateBody() {

        SetListParentActive(listInServicesList, true);
        SetListParentActive(listInRoomsList, true);


        listInServicesList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Services");

        listInRoomsList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Rooms");

        UpdatePanelContents(listInServicesList, "FocusableServices");
        UpdatePanelContents(listInRoomsList, "FocusableRooms");

    }

    private void UpdateHeader() {
        var title = leftNavTitle.GetComponent<TextMeshProUGUI>();
        string buildingName = currentlySelectedBuilding.transform.name;
        title.SetText(buildingName);

        var description = leftNavDescription.GetComponent<TextMeshProUGUI>();
        bool descriptionAvailable = false;

        foreach (var item in mapData.buildingData) {
            if (item.buildingName.Trim().Equals(buildingName.Trim())) {
                description.SetText(item.description);
                descriptionAvailable = true;
            }
        }

        if (!descriptionAvailable) {
            description.SetText("");
        }
    }

    private void UpdatePanelContents(Transform list, string transformName) {

        var focusTransform = currentlySelectedBuilding.Find(transformName);

        if (focusTransform == null) {
            SetListParentActive(list, false);
            // Debug.Log("No FocusableServices transform.");
            return;
        }

        List<Transform> focusableServiceRooms = focusTransform.Cast<Transform>().ToList();

        if (focusableServiceRooms != null && focusableServiceRooms.Count == 0) {
            SetListParentActive(list, false);
            // Debug.Log("No focusable services.");
            return;
        }
        
        ClearChildren(list);

        for (int i = 0; i < focusableServiceRooms.Count; i++) {
            CreateButton(focusableServiceRooms[i], list, roomsButtonPrefab);
        }
    }

    public void HandleUIFullscreen() {
        uIFullscreen = !uIFullscreen;


        // if (uIFullscreen) {

        // } else {

        // }

        leftNav
            .DOMoveX(distance, duration)
            .SetEase(ease);

        distance *= -1;
    }

    private void SetListParentActive(Transform list, bool parentState) {
        // if (list.childCount == 0) {
        //     return;
        // }

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
            CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.FocusedView);
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
        // search.SetActive(false);
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

                if (child.Find("FocusableRooms") != null) {
                    CreateButton(child, listInServicesList, roomsButtonPrefab);
                } else {
                    CreateButton(child, listInRoomsList, roomsButtonPrefab);
                }

            } 
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
