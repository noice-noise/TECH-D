using UnityEngine;
using TMPro;

public class CameraModeDropdown : MonoBehaviour {
    public int currentValue;

    private TMP_Dropdown dropdown;

    private void Awake() {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void OnValueChanged() {
        CameraManager.Instance.SwitchCameraMode(dropdown.value);
    }
}