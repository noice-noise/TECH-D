using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class AutoCompleteComboBoxExtension : MonoBehaviour {
    
    public AutoCompleteComboBox search;
    public InputField inputField;
    public GameObject suggestionOverlay;
    
    public Button clearButton;

    private void OnEnable() {
        ShowSuggestionPanel();
    }    

    private void Start() {
        clearButton.onClick.AddListener(delegate { ShowSuggestionPanel(); });
    }

    private void Update() {
        HandleFocused();
    }

    private void HandleFocused() {
        if (inputField.isFocused) {
            HandleEmptyInputField();
        }
    }

    private void HandleEmptyInputField() {
        ShowSuggestionPanel();
    }

    private void ShowSuggestionPanel() {
        if (!suggestionOverlay.activeSelf) {
            search.ToggleDropdownPanel(false);
        }
    }
}
