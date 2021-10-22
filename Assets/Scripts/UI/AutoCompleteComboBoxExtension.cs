using UnityEngine;
using UnityEngine.UI.Extensions;

public class AutoCompleteComboBoxExtension : MonoBehaviour {
    private void Start() {
        // show drop down panel
        GetComponent<AutoCompleteComboBox>().ToggleDropdownPanel(false);
    }
}
