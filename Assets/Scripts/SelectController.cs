using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ui {
    public GameObject panel;
    public Text name;
    public Image icon;
}

public class SelectController : MonoBehaviour {
    [SerializeField] private LayerMask interactableLayermask = 0;
    [SerializeField] private GameObject selectObject = null;
    [SerializeField] private Ui ui = new Ui();

    private Camera cam = null;
    private UnitObject selectedUnit = null;

    private static void Error(string msg) {
        Debug.LogError(msg);
        Debug.Break();
    }

    private void Start() {
        if (interactableLayermask == 0) Error("LAYERMASK IS NULL");
        if (Camera.main != null) cam = Camera.main;
        if(ui.panel == null) Error("INFO PANEL IS NULL");
        if(selectObject == null) Error("SELECT OBJET IS NULL");
    }

    private void Update() {
        RaycastCheck();
    }

    private void RaycastCheck() {
        if (!Input.GetMouseButtonDown(0)) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayermask)) {
            Select(hit);
        }
        DeSelect();
    }

    private void DeSelect() {
        selectedUnit = null;
        ui.panel.SetActive(false);
        selectObject.SetActive(false);
    }

    private void Select(RaycastHit hit) {
        if(hit.transform.GetComponentInParent<UnitObject>() == null) Error("TRIED TO SELECT NULL UNIT");
        selectedUnit = hit.transform.GetComponentInParent<UnitObject>();
        ui.panel.SetActive(true);
        ui.name.text = selectedUnit.Unit.name;
        ui.icon.sprite = selectedUnit.Unit.icon;
        selectObject.transform.position = selectedUnit.transform.position;
        selectObject.SetActive(true);
    }
}