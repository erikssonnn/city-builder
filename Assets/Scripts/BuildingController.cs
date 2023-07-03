using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BuildingController : MonoBehaviour {
    [SerializeField] private GameObject placeholder = null;
    [SerializeField] private LayerMask lm = 0;
    [SerializeField] private Material matGreen, matRed = null;
    [SerializeField] private Transform buildingsParent = null;

    [FormerlySerializedAs("units")] [SerializeField]
    private Building[] buildings = null;
    private MapController mapController = null;
    public bool PlacingBuilding { get; private set; } = false;

    private Camera cam = null;
    private List<Vector3Int> positions = new List<Vector3Int>();
    private ResourceController resourceController = null;
    private SelectController selectController = null;
    private PopulationController populationController = null;
    private Building selectedBuilding = null;

    private void Start() {
        NullObjectCheck();
        PlaceStartBuilding();
    }

    private void NullObjectCheck() {
        mapController = FindObjectOfType<MapController>();
        if (mapController == null) {
            throw new System.Exception("Cant find MapController instance!");
        }

        resourceController = FindObjectOfType<ResourceController>();
        if (resourceController == null) {
            throw new System.Exception("Cant find ResourceController instance!");
        }
        
        selectController = FindObjectOfType<SelectController>();
        if (selectController == null) {
            throw new System.Exception("Cant find SelectController instance!");
        }
        
        populationController = FindObjectOfType<PopulationController>();
        if (populationController == null) {
            throw new System.Exception("Cant find PopulationController instance!");
        }

        if (Camera.main != null) {
            cam = Camera.main;
        }
    }

    private void PlaceStartBuilding() {
        BuildingObject buildingObject = placeholder.GetComponent<BuildingObject>();
        buildingObject.Building = buildings[3];
        PlaceBuilding(buildingObject, Vector3.zero, Quaternion.identity);
    }

    public void HoverBuildingEvent(int index) {
        Building selectedBuilding = buildings[index];
        if (selectedBuilding == null) {
            throw new System.Exception("SelectedBuilding is null!");
        }

        BuildingCost cost = selectedBuilding.cost;
        resourceController.DisplayResourceCost(cost);
    }

    public void LeaveBuildingEvent() {
        resourceController.DisplayResourceCost(new BuildingCost(0, 0, 0));
    }

    public void NewBuildingEvent(int index) {
        selectController.DeSelect();
        
        selectedBuilding = buildings[index];
        if (selectedBuilding == null) {
            throw new System.Exception("SelectedBuilding is null!");
        }

        BuildingCost cost = selectedBuilding.cost;
        if (!resourceController.EnoughResources(cost)) return;

        placeholder.GetComponent<BuildingObject>().Building = selectedBuilding;
        placeholder.GetComponentInChildren<MeshFilter>().sharedMesh =
            selectedBuilding.prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        placeholder.SetActive(true);
        PlacingBuilding = true;
    }

    private void LateUpdate() {
        IsPlacingBuilding();
    }

    private void PlaceBuilding(BuildingObject buildingObject, Vector3 pos, Quaternion rot) {
        GameObject newObj = Instantiate(buildingObject.Building.prefab, placeholder.transform.position,
            placeholder.transform.rotation, buildingsParent);

        Building b = buildingObject.Building;
        if (b.capacity > 0) {
            populationController.ChangeCapacity(b.capacity);
        }
        
        List<Vector3Int> temp = buildingObject.GetBuildingPositions(pos, rot);
        foreach (Vector3Int t in temp.Where(t => !mapController.Map.Contains(t))) {
            newObj.GetComponent<BuildingObject>().Positions.Add(t);
            mapController.Map.Add(t);
        }
    }
    
    private void IsPlacingBuilding() {
        if (!PlacingBuilding) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
            PositionBuilding(placeholder, hit.point);
        }

        if (EventSystem.current.IsPointerOverGameObject(-1)) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            placeholder.transform.eulerAngles += new Vector3(0f, 90f, 0f);
        }

        if (Input.GetMouseButtonDown(1)) {
            CancelPlacingBuilding();
        }

        if (selectedBuilding == null) {
            throw new System.Exception("SelectedBuilding is null!");
        }

        BuildingCost cost = selectedBuilding.cost;
        if (!resourceController.EnoughResources(cost)) {
            CancelPlacingBuilding();
        }

        if (!Input.GetMouseButtonDown(0) || !CanPlace()) return;
        BuildingObject buildingObject = placeholder.GetComponent<BuildingObject>();
        Transform placeholderT = placeholder.transform;

        PlaceBuilding(buildingObject, placeholderT.position, placeholderT.rotation);
        
        resourceController.ChangeResource(cost);
        PlacingBuilding = false;
        placeholder.SetActive(false);
        selectedBuilding = null;
    }

    private void CancelPlacingBuilding() {
        PlacingBuilding = false;
        placeholder.SetActive(false);
    }

    private bool CanPlace() {
        if (positions.Count <= 0) return false;
        return !mapController.IntersectsMapPos(positions);
    }

    private void PositionBuilding(GameObject obj, Vector3 pos) {
        obj.transform.position = new Vector3Int(Mathf.FloorToInt(pos.x), 0, Mathf.FloorToInt(pos.z));
        Transform objT = obj.transform;
        positions = obj.GetComponent<BuildingObject>().GetBuildingPositions(objT.position, objT.rotation);

        MeshRenderer ren = obj.GetComponentInChildren<MeshRenderer>();
        Material[] materials = ren.sharedMaterials;

        for (int i = 0; i < materials.Length; i++) {
            materials[i] = CanPlace() ? Instantiate(matGreen) : Instantiate(matRed);
        }

        ren.materials = materials;
    }
}