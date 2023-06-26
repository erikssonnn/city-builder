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
    private BuildingObject selectedBuilding = null;
    private MapController mapController = null;

    private void Start() {
        if (lm == 0) {
            throw new System.Exception("Layermask is null!");
        }

        if (Camera.main != null) {
            cam = Camera.main;
        }

        if (highlightObject == null) {
            throw new System.Exception("Selector object is null!");
        }
        
        mapController = FindObjectOfType<MapController>();
    }

    private void Update() {
        RaycastCheck();
    }
    
    public void DeleteBuildingEvent() {
        if (selectedBuilding == null) {
            throw new System.Exception("Tried to delete a null building!");
        }
        if (selectedBuilding.Positions.Count == 0) {
            throw new System.Exception("Building has zero occupied positions!");
        }
        
        foreach (Vector3Int pos in selectedBuilding.Positions) {
            if (mapController.Map.Contains(pos)) {
                mapController.Map.Remove(pos);
            }
        }
        Destroy(selectedBuilding.gameObject);
        DeSelect();
        selectedBuilding = null;
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
        selectedBuilding = null;
        ui.panel.SetActive(false);
    }

    private void Select(RaycastHit hit) {
        if (hit.transform.GetComponentInParent<BuildingObject>() == null) {
            throw new System.Exception("Cant find UnitObject on hit transform!");
        }
        
        selectedBuilding = hit.transform.GetComponentInParent<BuildingObject>();
        ui.panel.SetActive(true);
        ui.name.text = selectedBuilding.Building.name;
        ui.icon.sprite = selectedBuilding.Building.icon;

        Transform selectedBuildingTransform = selectedBuilding.transform;
        highlightObject.transform.SetPositionAndRotation(selectedBuildingTransform.position, selectedBuildingTransform.rotation);
        highlightObject.GetComponentInChildren<MeshFilter>().sharedMesh =
            selectedBuilding.transform.GetComponentInChildren<MeshFilter>().sharedMesh;
        highlightObject.SetActive(true);
    }
}