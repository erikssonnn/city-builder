using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingController : MonoBehaviour {
    [SerializeField] private GameObject placeholder = null;
    [SerializeField] private LayerMask lm = 0;
    [SerializeField] private Material matGreen, matRed = null;
    [SerializeField] private Transform buildingsParent = null;
    [SerializeField] private Unit[] units = null;

    private MapController mapController = null;
    private bool placingBuilding = false;
    private Camera cam = null;
    private List<Vector3Int> positions = new List<Vector3Int>();

    private void Start() {
        mapController = FindObjectOfType<MapController>();
        if (mapController == null) throw new Exception("Cant find MapController instance!");
        if (Camera.main != null) cam = Camera.main;
    }

    public void NewBuildingEvent(int index) {
        Unit selectedBuilding = units[index];
        if (selectedBuilding == null) throw new Exception("SelectedBuilding is null or out of range!");

        placeholder.GetComponent<UnitObject>().Unit = selectedBuilding;
        placeholder.GetComponentInChildren<MeshFilter>().sharedMesh =
            selectedBuilding.buildingPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        placeholder.SetActive(true);
        placingBuilding = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T) && !placingBuilding) {
            NewBuildingEvent(0);
        }
    }

    private void LateUpdate() {
        if (!placingBuilding) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
            PositionBuilding(placeholder, hit.point);
        }

        if (EventSystem.current.IsPointerOverGameObject(-1)) {
            return;
        }
        
        if (Input.GetMouseButtonDown(1)) {
            placingBuilding = false;
            placeholder.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && CanPlace()) {
            UnitObject unitObject = placeholder.GetComponent<UnitObject>();
            GameObject newObj = Instantiate(unitObject.Unit.buildingPrefab, placeholder.transform.position, Quaternion.identity, buildingsParent);
            
            List<Vector3Int> temp = unitObject.GetBuildingPositions(placeholder.transform.position);
            foreach (Vector3Int t in temp.Where(t => !mapController.Map.Contains(t))) {
                newObj.GetComponent<UnitObject>().Positions.Add(t);
                mapController.Map.Add(t);
            }

            placingBuilding = false;
            placeholder.SetActive(false);
        }
    }

    private bool CanPlace() {
        if (positions.Count <= 0) return false;
        if (mapController.IntersectsMapPos(positions)) {
            return false;
        }

        return true;
    }

    private void PositionBuilding(GameObject obj, Vector3 pos) {
        obj.transform.position = new Vector3Int(Mathf.FloorToInt(pos.x), 0, Mathf.FloorToInt(pos.z));
        positions = obj.GetComponent<UnitObject>().GetBuildingPositions(obj.transform.position);

        MeshRenderer ren = obj.GetComponentInChildren<MeshRenderer>();
        Material[] materials = ren.sharedMaterials; 

        for (int i = 0; i < materials.Length; i++) {
            materials[i] = CanPlace() ? Instantiate(matGreen) : Instantiate(matRed);
        }

        ren.materials = materials; 
    }
}