using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private float panSpeed = 0.0f;
    [SerializeField] private float scrollPanSpeed = 0.0f;
    [SerializeField] private float scrollSpeed = 0.0f;

    private MapController mapController = null;
    private Camera cam = null;
    private const float downScale = 0.1f;
    private Transform t = null;
    private Vector2Int mapLimit = Vector2Int.zero;
    
    private void Start() {
        t = transform;
        mapController = FindObjectOfType<MapController>();
        if (mapController == null) {
            throw new System.Exception("MapController is null!");
        }
        cam = Camera.main;
        if (cam == null) {
            throw new System.Exception("Cant find main camera!");
        }
        mapLimit = new Vector2Int(mapController.MapSize.x, mapController.MapSize.y);
    }

    private void Update() {
        if (Input.GetMouseButton(2)) {
            ScrollMovement();
            return;
        }

        Zoom();
        Movement();
    }

    private void Zoom() {
        float size = cam.orthographicSize;
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        size -= scroll * scrollSpeed * Time.fixedDeltaTime;
        size = Mathf.Clamp(size, 15, 40);
        cam.orthographicSize = size;
    }

    private void ScrollMovement() {
        float x = Input.GetAxisRaw("Mouse X");
        float y = Input.GetAxisRaw("Mouse Y");
        Vector3 pos = transform.position -= new Vector3(x, 0, y) *
            (scrollPanSpeed * Time.fixedDeltaTime * cam.orthographicSize * downScale);
        pos.x = Mathf.Clamp(pos.x, -mapLimit.x, mapLimit.x);
        pos.z = Mathf.Clamp(pos.z, -mapLimit.y, mapLimit.y);
        t.position = pos;
    }

    private void Movement() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 pos = transform.position +=
            new Vector3(x, 0, y) * (panSpeed * Time.fixedDeltaTime * cam.orthographicSize * downScale);
        pos.x = Mathf.Clamp(pos.x, -mapLimit.x, mapLimit.x);
        pos.z = Mathf.Clamp(pos.z, -mapLimit.y, mapLimit.y);
        t.position = pos;
    }
}