using UnityEngine;

public class DebugController : MonoBehaviour {
    [SerializeField] private GameObject unitObj = null;
    [SerializeField] private LayerMask lm = 0;

    private Camera cam = null;
    
    private void Start() {
        if (unitObj == null) {
            throw new System.Exception("UnitObj is null on " + name);
        }
        cam = Camera.main;
        if (cam == null) {
            throw new System.Exception("Camera cant be found!");
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
                Instantiate(unitObj, hit.point, Quaternion.identity);
            }
        }
    }
}
