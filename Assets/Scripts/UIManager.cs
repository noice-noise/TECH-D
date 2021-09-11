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

    public Transform listInBuilding;

    public Transform listInServicesList;
    public Transform listInRoomsList;
    public Transform searchParent;

    [Header("Instantiation")]
    public GameObject roomsButtonPrefab;
    public GameObject quickSearchPrefab;

    private Transform currentlySelectedBuilding;
    private IEnumerable<Transform> worldChildren;

    private void Start() {
        DisplayBuildingPane();
        InitQuickSearch();
    }

    public void UpdateMapUI() {
        currentlySelectedBuilding = Navigation.Instance.currentlyFocusedBuilding;
        UpdateHeader();
        UpdateBody();
    }

    private void UpdateBody() {
        UpdateServiceContents();
        UpdateRoomContents();
    }

    private void UpdateHeader() {
        var text = leftNavTitle.GetComponent<TextMeshProUGUI>();
        text.SetText(currentlySelectedBuilding.transform.name);
    }

    private void UpdateRoomContents() {
        var focusTransform = currentlySelectedBuilding.Find("FocusableRooms");

        if (focusTransform == null) {
            Debug.Log("No focusable rooms.");
            return;
        }

        List<Transform> focusableLectureRooms = focusTransform.Cast<Transform>().ToList();

        if (focusableLectureRooms != null && focusableLectureRooms.Count == 0)
            return;
        
        ClearChildren(listInRoomsList);

        for (int i = 0; i < focusableLectureRooms.Count; i++) {
            CreateButton(focusableLectureRooms[i], listInRoomsList, roomsButtonPrefab);
        }
    }

    private void UpdateServiceContents() {
        var focusTransform = currentlySelectedBuilding.Find("FocusableServices");

        if (focusTransform == null) {
            Debug.Log("No focusable services.");
            return;
        }

        List<Transform> focusableServiceRooms = focusTransform.Cast<Transform>().ToList();

        if (focusableServiceRooms != null && focusableServiceRooms.Count == 0)
            return;
        
        ClearChildren(listInServicesList);

        for (int i = 0; i < focusableServiceRooms.Count; i++) {
            CreateButton(focusableServiceRooms[i], listInServicesList, roomsButtonPrefab);
        }
    }

    private void CreateButton(Transform transformSource, Transform targetParent, GameObject buttonPrefab) {

        GameObject newButton = Instantiate(buttonPrefab, targetParent, false);
        newButton.transform.SetParent(targetParent);
        newButton.transform
            .Find("Text")
            .GetComponent<TextMeshProUGUI>()
            .SetText(transformSource.transform.name);
        newButton.GetComponent<Room>().buildingReference = transformSource;
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
        
        List<string> list = new List<string>();

        foreach(Transform child in worldChildren) {
            if (child.CompareTag("SelectableBuilding")) {
                list.Add(child.name);
            }
        }

        prefab.GetComponent<AutoCompleteComboBox>().AvailableOptions = list;
        GameObject search = Instantiate(quickSearchPrefab, searchParent, false);
    }

    public void DisplayBuildingPane() {
        ClearChildren(listInRoomsList);
        ClearChildren(listInServicesList);

        worldChildren = world.transform.Cast<Transform>();

        foreach(Transform child in worldChildren) {
            if (child.CompareTag("SelectableBuilding")) {
                CreateButton(child, listInServicesList, roomsButtonPrefab);
            }
        }
    }
}
