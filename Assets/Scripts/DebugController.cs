using UnityEngine;

public class DebugController : MonoBehaviour {
    [SerializeField] private LayerMask lm = 0;

    private Camera cam = null;
    private PopulationController populationController = null;

    private void Start() {
        NullObjectCheck();
    }

    private void NullObjectCheck() {
        populationController = FindObjectOfType<PopulationController>();
        if (populationController == null) {
            throw new System.Exception("Cant find PopulationController instance!");
        }

        cam = Camera.main;
        if (cam == null) {
            throw new System.Exception("Camera cant be found!");
        }
    }

    private void Update() {
        if (!Input.GetKeyDown(KeyCode.P)) return;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
            populationController.CreateUnit(hit.point);
        }
    }
}