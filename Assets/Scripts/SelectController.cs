using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SelectController : MonoBehaviour {
    [SerializeField] private LayerMask lm = 0;
    [FormerlySerializedAs("selectObject")] [SerializeField] private GameObject highlightObject = null;

    private Camera cam = null;
    private BuildingObject selectedBuilding = null;
    private BuildingController buildingController = null;
    private MapController mapController = null;
    private PopulationController populationController = null;
    private UiController ui = null;

    private void Start() {
        ui = UiController.Instance;
        NullObjectCheck();
    }

    private void NullObjectCheck() {
        if (lm == 0) {
            throw new System.Exception("Layermask is null!");
        }
        if (Camera.main != null) {
            cam = Camera.main;
        }
        if (highlightObject == null) {
            throw new System.Exception("Selector object is null!");
        }
        buildingController = FindObjectOfType<BuildingController>();
        if (buildingController == null) {
            throw new System.Exception("Cant find BuildingController instance!");
        }
        mapController = FindObjectOfType<MapController>();
        if (mapController == null) {
            throw new System.Exception("Cant find MapController instance!");
        }
        populationController = FindObjectOfType<PopulationController>();
        if (populationController == null) {
            throw new System.Exception("Cant find PopulationController instance!");
        }
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
        
        foreach (Vector3Int pos in selectedBuilding.Positions.Where(pos => mapController.Map.Contains(pos))) {
            mapController.Map.Remove(pos);
        }
        
        populationController.ChangeCapacity(-selectedBuilding.Building.capacity);
        
        Destroy(selectedBuilding.gameObject);
        DeSelect();
        selectedBuilding = null;
    }

    private void RaycastCheck() {
        if (!Input.GetMouseButtonDown(0)) return;
        if (buildingController.PlacingBuilding) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
            Select(hit);
            return;
        }
        
        if (!EventSystem.current.IsPointerOverGameObject(-1)) {
            DeSelect();
        }
    }

    public void DeSelect() {
        highlightObject.SetActive(false);
        selectedBuilding = null;
        ui.selectPanel.SetActive(false);
    }

    private void Select(RaycastHit hit) {
        if (hit.transform.GetComponentInParent<BuildingObject>() == null) {
            throw new System.Exception("Cant find UnitObject on hit transform!");
        }
        
        selectedBuilding = hit.transform.GetComponentInParent<BuildingObject>();
        ui.selectPanel.SetActive(true);
        ui.selectedNameText.text = selectedBuilding.Building.name;
        ui.selectImage.sprite = selectedBuilding.Building.icon;

        Transform selectedBuildingTransform = selectedBuilding.transform;
        highlightObject.transform.SetPositionAndRotation(selectedBuildingTransform.position, selectedBuildingTransform.rotation);
        highlightObject.GetComponentInChildren<MeshFilter>().sharedMesh =
            selectedBuilding.transform.GetComponentInChildren<MeshFilter>().sharedMesh;
        highlightObject.SetActive(true);
    }
}