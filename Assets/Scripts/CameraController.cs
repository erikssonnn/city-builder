using System;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float panThickness = 0.0f;

    private void Update() {
        Movement();
    }

    private void Movement() {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panThickness) {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panThickness) {
            pos.z -= speed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panThickness) {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panThickness) {
            pos.x -= speed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
