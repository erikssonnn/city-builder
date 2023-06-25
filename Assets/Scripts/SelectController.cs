using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class Ui {
    public GameObject panel;
    public Text name;
    public Image icon;
}

public class SelectController : MonoBehaviour {
    [SerializeField] private LayerMask lm = 0;
    [FormerlySerializedAs("selectObject")] [SerializeField] private GameObject highlightObject = null;
    [SerializeField] private Ui ui = new Ui();

    private Camera cam = null;
    private UnitObject selectedUnit = null;
    private MapController mapController = null;

    private static void Error(string msg) {
        Debug.LogError(msg);
        Debug.Break();
    }

    private void Start() {
        if (lm == 0) Error("Layermask is null!");
        if (Camera.main != null) cam = Camera.main;
        if(highlightObject == null) Error("Selector object is null!");
        mapController = FindObjectOfType<MapController>();
    }

    private void Update() {
        RaycastCheck();
    }
    
    public void DeleteBuildingEvent() {
        if (selectedUnit == null) Error("Tried to delete a null building!");
        if (selectedUnit.Positions.Count == 0) Error("Building has zero occupied positions!");
        foreach (Vector3Int pos in selectedUnit.Positions) {
            if (mapController.Map.Contains(pos)) {
                mapController.Map.Remove(pos);
            }
        }
        Destroy(selectedUnit.gameObject);
        DeSelect();
        selectedUnit = null;
    }

    private void RaycastCheck() {
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
            Select(hit);
            return;
        }
        
        if (!EventSystem.current.IsPointerOverGameObject(-1)) {
            DeSelect();
        }
    }

    private void DeSelect() {
        highlightObject.SetActive(false);
        selectedUnit = null;
        ui.panel.SetActive(false);
    }

    private void Select(RaycastHit hit) {
        if(hit.transform.GetComponentInParent<UnitObject>() == null) Error("Cant find UnitObject on hit transform!");
        selectedUnit = hit.transform.GetComponentInParent<UnitObject>();
        ui.panel.SetActive(true);
        ui.name.text = selectedUnit.Unit.name;
        ui.icon.sprite = selectedUnit.Unit.icon;
        highlightObject.transform.position = selectedUnit.transform.position;
        highlightObject.GetComponentInChildren<MeshFilter>().sharedMesh =
            selectedUnit.transform.GetComponentInChildren<MeshFilter>().sharedMesh;
        highlightObject.SetActive(true);
    }
}