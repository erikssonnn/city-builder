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
    private bool placingBuilding = false;
    private Camera cam = null;
    private List<Vector3Int> positions = new List<Vector3Int>();
    private ResourceController resourceController = null;
    private Building selectedBuilding = null;

    private void Start() {
        mapController = FindObjectOfType<MapController>();
        if (mapController == null) {
            throw new System.Exception("Cant find MapController instance!");
        }

        resourceController = FindObjectOfType<ResourceController>();
        if (resourceController == null) {
            throw new System.Exception("Cant find ResourceController instance!");
        }

        if (Camera.main != null) {
            cam = Camera.main;
        }
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
        placingBuilding = true;
    }

    private void LateUpdate() {
        if (!placingBuilding) return;
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

        if (Input.GetMouseButtonDown(0) && CanPlace()) {
            BuildingObject buildingObject = placeholder.GetComponent<BuildingObject>();
            Transform placeholderT = placeholder.transform;
            GameObject newObj = Instantiate(buildingObject.Building.prefab, placeholder.transform.position,
                placeholder.transform.rotation, buildingsParent);

            List<Vector3Int> temp = buildingObject.GetBuildingPositions(placeholderT.position, placeholderT.rotation);
            foreach (Vector3Int t in temp.Where(t => !mapController.Map.Contains(t))) {
                newObj.GetComponent<BuildingObject>().Positions.Add(t);
                mapController.Map.Add(t);
            }

            print("cost: " + cost.wood);
            resourceController.ChangeResource(cost);

            placingBuilding = false;
            placeholder.SetActive(false);
        }
    }

    private void CancelPlacingBuilding() {
        placingBuilding = false;
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