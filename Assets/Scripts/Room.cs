using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour {
    public Transform buildingReference;

    private void Start() {
        if (buildingReference == null) {
            var text = transform.Find("Text").GetComponent<Text>();
            Debug.Log(text.text);
            string capitalizedString = text.text[0].ToString().ToUpper() + text.text.Substring(1);

            Transform targetBuilding = UIManager.Instance.world.transform.Find(capitalizedString);
            
            if (targetBuilding != null) {
                buildingReference = targetBuilding;
            }
        }
    }

    public void SwitchCameraTarget() {
        if (buildingReference == null) {
            Debug.Log("Building reference not found.");
        }

        Navigation.Instance.SelectFromButton(buildingReference);
    }
}
