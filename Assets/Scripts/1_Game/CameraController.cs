using UnityEngine;

namespace _1_Game {
    public class CameraController : MonoBehaviour {
        [SerializeField] private float panSpeed = 0.0f;
        [SerializeField] private float scrollPanSpeed = 0.0f;
        [SerializeField] private float scrollSpeed = 0.0f;

        private Camera cam = null;
        private const float downScale = 0.1f;
        private Vector2Int mapLimit = Vector2Int.zero;
    
        private void Start() {
            cam = Camera.main;
            if (cam == null) {
                throw new System.Exception("Cant find main camera!");
            }
            mapLimit = new Vector2Int(100, 100);
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
            size = Mathf.Clamp(size, 10, 30);
            cam.orthographicSize = size;
        }

        private void ScrollMovement() {
            float x = Input.GetAxisRaw("Mouse X");
            float y = Input.GetAxisRaw("Mouse Y");
            Vector3 pos = transform.position -= new Vector3(x, 0, y) *
                                                (scrollPanSpeed * Time.fixedDeltaTime * cam.orthographicSize * downScale);
            pos.x = Mathf.Clamp(pos.x, -mapLimit.x, mapLimit.x);
            pos.z = Mathf.Clamp(pos.z, -mapLimit.y, mapLimit.y);
            transform.position = pos;
        }

        private void Movement() {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector3 pos = transform.position +=
                new Vector3(x, 0, y) * (panSpeed * Time.fixedDeltaTime * cam.orthographicSize * downScale);
            pos.x = Mathf.Clamp(pos.x, -mapLimit.x, mapLimit.x);
            pos.z = Mathf.Clamp(pos.z, -mapLimit.y, mapLimit.y);
            transform.position = pos;
        }
    }
}