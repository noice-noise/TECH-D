using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.UI.Extensions;

public class UIManager : Singleton<UIManager>
{
    [Header("Hierarchy")]
    public GameObject mainCanvas;
    public GameObject world;

    [Header("UI")]
    public Transform leftNav;
    public Transform leftNavTitle;
    public Transform leftNavDescription;

    // public Transform listInBuilding;

    public Transform listInServicesList;
    public Transform listInRoomsList;
    public Transform searchParent;

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

    private void UpdateBody() {

        SetListParentActive(listInServicesList, true);
        SetListParentActive(listInRoomsList, true);

        // reset services name (from building)
        listInServicesList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Services");

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
            Debug.Log("No FocusableServices transform.");
            return;
        }

        List<Transform> focusableServiceRooms = focusTransform.Cast<Transform>().ToList();

        if (focusableServiceRooms != null && focusableServiceRooms.Count == 0) {
            SetListParentActive(list, false);
            Debug.Log("No focusable services.");
            return;
        }
        
        ClearChildren(list);

        for (int i = 0; i < focusableServiceRooms.Count; i++) {
            CreateButton(focusableServiceRooms[i], list, roomsButtonPrefab);
        }
    }

    // private void UpdateRoomContents() {
    //     var focusTransform = currentlySelectedBuilding.Find("FocusableRooms");

    //     if (focusTransform == null) {
    //         Debug.Log("No focusable rooms.");
    //         return;
    //     }

    //     List<Transform> focusableLectureRooms = focusTransform.Cast<Transform>().ToList();

    //     if (focusableLectureRooms != null && focusableLectureRooms.Count == 0)
    //         return;
        
    //     ClearChildren(listInRoomsList);

    //     for (int i = 0; i < focusableLectureRooms.Count; i++) {
    //         CreateButton(focusableLectureRooms[i], listInRoomsList, roomsButtonPrefab);
    //     }
    // }

    // private void UpdateServiceContents() {

    //     var focusTransform = currentlySelectedBuilding.Find("FocusableServices");

    //     if (focusTransform == null) {
    //         SetListParentActive(listInServicesList, false);
    //         Debug.Log("No FocusableServices transform.");
    //         return;
    //     }

    //     List<Transform> focusableServiceRooms = focusTransform.Cast<Transform>().ToList();

    //     if (focusableServiceRooms != null && focusableServiceRooms.Count == 0) {
    //         SetListParentActive(listInServicesList, false);
    //         Debug.Log("No focusable services.");
    //         return;
    //     }

        
    //     ClearChildren(listInServicesList);

    //     for (int i = 0; i < focusableServiceRooms.Count; i++) {
    //         CreateButton(focusableServiceRooms[i], listInServicesList, roomsButtonPrefab);
    //     }
    // }

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

    public void InitQuickSearch() {
        var prefab = quickSearchPrefab;

        List<string> searchList = new List<string>();

        foreach(Transform gameObject in worldChildren) {
            if (gameObject.CompareTag("SelectableBuilding")) {
                searchList.Add(gameObject.name);

                List<string> focusableServices = Parse(gameObject, "FocusableServices");
                searchList.AddRange(focusableServices);

                List<string> focusableRooms = Parse(gameObject, "FocusableRooms");
                searchList.AddRange(focusableRooms);
            }
        }

        List<string> availableOptions = prefab.GetComponent<AutoCompleteComboBox>().AvailableOptions = searchList;    
        GameObject search = Instantiate(quickSearchPrefab, searchParent, false);

        // Transform itemsList = search.transform.Find("Overlay").transform.Find("ScrollPanel").transform.Find("Items");
        // transform.Find("Items");
        // Debug.Log(itemsList.name);

        // // foreach(Transform item in itemsList) {
        // //     item.GetComponentInChildren<RoomButton>().buildingReference 
        // //     Debug.Log(item.name);
        // // }
    }

    private List<string> Parse(Transform source, string focusableStringID) {
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
        DisplayBuildingPane();
        CameraManager.Instance.SwitchCameraMode(CameraManager.CameraState.MapView);
    }

    private void DisplayBuildingPane() {
        ClearChildren(listInRoomsList);
        SetListParentActive(listInRoomsList, false);

        ClearChildren(listInServicesList);
        SetListParentActive(listInServicesList, true);

        leftNavTitle.GetComponent<TextMeshProUGUI>().SetText("CIT University");
        leftNavDescription.GetComponentInChildren<TextMeshProUGUI>().SetText("All Buildings");

        listInServicesList.parent.transform.parent.transform.Find("Title").GetComponentInChildren<TextMeshProUGUI>().SetText("Building");

        foreach(Transform child in worldChildren) {
            if (child.CompareTag("SelectableBuilding")) {
                CreateButton(child, listInServicesList, roomsButtonPrefab);
            }
        }
    }
}
