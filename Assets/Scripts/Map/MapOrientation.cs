using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapOrientation : MonoBehaviour {
    
    public enum Orientation
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3,
        NorthEast = 4,
        SouthEast = 5,
        SouthWest = 6,
        NorthWest = 7
    }

    // responsible map camera will select these references
    public Transform northWestTransform;
    public Transform southEastTransform;

    public int currentValue;
    private TMP_Dropdown dropdown;
    private List<string> optionsList;

    public Orientation orientation;
    [SerializeField]
    private bool allowMapOrientation;

    private void Awake() {

        PopulateOptionsList();

        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(optionsList);
        // dropdown.;
    }

    private void PopulateOptionsList() {
        optionsList = new List<string>();
        optionsList.Add("North-West");
        optionsList.Add("South-East");
    }

    private void Start() {
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    private void Update() {

    }

    private void HandleOrientation() {
        int mapViewCameraInt = 0;

        // implementation is currently for NW and SE, others are authored for extensibility
        switch (orientation)
        {
            case Orientation.North:
            case Orientation.South:
            case Orientation.East:
            case Orientation.West:
            case Orientation.NorthEast:
            case Orientation.SouthEast:
                CameraManager.Instance.SwitchVirtualCameraTarget(mapViewCameraInt, southEastTransform);
                break;
            case Orientation.SouthWest:
            case Orientation.NorthWest:
                CameraManager.Instance.SwitchVirtualCameraTarget(mapViewCameraInt, northWestTransform);
                break;
            default:
                break;
        }

        Debug.Log("Wew?");
        CameraManager.Instance.SwitchCameraModeWith(mapViewCameraInt);
    }

    public void DropdownValueChanged() {
        // implementation is currently for NW and SE, others are authored for extensibility
        switch (dropdown.value)
        {
            case 0:
                orientation = Orientation.SouthEast;
                break;
            case 1:
                orientation = Orientation.NorthWest;
                break;
            default:
                Debug.LogError("Value invalid.");
                break;
        }

        HandleOrientation();
    }
}