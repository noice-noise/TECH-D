using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    public Material defaultMaterial;
    public Material highlightMaterial;
    // [SerializeField] private Material originalMaterial;

    private Transform _selection;

    private void Update() {

        if (_selection != null) {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(ray, transform.forward, Color.yellow);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {

            var selection = hit.transform;

            if (selection.CompareTag(selectableTag)) {
                var selectionRenderer = selection.GetComponent<Renderer>(); 
                if (selectionRenderer != null) {
                    selectionRenderer.material = highlightMaterial;
                }
                _selection = selection;
            }
        }
    }
}
