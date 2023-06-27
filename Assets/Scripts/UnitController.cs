using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class UnitController : MonoBehaviour {
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private float rotSpeed = 0.0f;
    [SerializeField] private LayerMask lm = 0;

    private NavMeshAgent agent = null;
    private int index = -1;
    private float velMag = 0;
    private readonly Dictionary<int, Vector3> path = new Dictionary<int, Vector3>();
    private bool isCreatingPath = false;
    private Camera cam = null;
    private Transform t = null;
    private MapController mapController = null;
    private const float downScale = 0.1f;

    private void OnDrawGizmos() {
        if (path.Count <= 0 || index == -1) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(path[index], Vector3.one);

        if (path.Count <= 0) return;
        foreach (KeyValuePair<int, Vector3> node in path) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(node.Value, Vector3.one * 0.5f);
        }
    }

    private void Start() {
        t = transform;
        mapController = FindObjectOfType<MapController>();
        if (mapController == null) {
            throw new System.Exception("Could not find MapController");
        }

        cam = Camera.main;
        if (cam == null)
            throw new System.Exception("Could not find Main Camera!");
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            throw new System.Exception("Could not find NavMeshAgent on " + name);
        RandomizeClothes();
        StartCoroutine(CreatePath(GetRandomPos()));
    }

    private void Update() {
        // TODO: debug, remove
        if (Input.GetKeyDown((KeyCode.O))) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, lm)) {
                StartCoroutine(CreatePath(hit.point));
            }
        }

        CheckStuck();
        if (path.Count <= 0 || index == -1) return;

        Vector3 lookPos = path[index] - t.position;
        lookPos.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookPos);
        t.rotation = Quaternion.Slerp(t.rotation, rot, Time.fixedDeltaTime * rotSpeed);

        t.Translate(t.TransformDirection(Vector3.forward) * (speed * downScale * Time.fixedDeltaTime), Space.World);

        float dist = Vector3.Distance(t.position, path[index]);
        if (dist > 0.5f) return;
        if (index >= path.Count - 1) {
            index = -1;
            StartCoroutine(CreatePath(GetRandomPos()));
            return;
        }

        index++;
    }

    private Vector3 GetRandomPos() {
        Vector2 size = mapController.MapSize;

        Vector3 randomPos = Random.insideUnitCircle * 10f;
        Vector3 ret = transform.position + new Vector3(randomPos.x, 0f, randomPos.y);

        ret.x = Mathf.Clamp(ret.x, -size.x, size.x);
        ret.z = Mathf.Clamp(ret.z, -size.y, size.y);

        return ret;
    }


    private IEnumerator CreatePath(Vector3 targetPos) {
        if (isCreatingPath) {
            StopCoroutine(CreatePath(targetPos));
        }

        isCreatingPath = true;

        index = -1;
        if (path.Count > 0) {
            path.Clear();
        }

        agent.enabled = true;
        agent.SetDestination(targetPos);
        agent.isStopped = true;
        while (agent.pathPending) {
            yield return new WaitForEndOfFrame();
        }

        if (agent.pathPending || agent.path.status != NavMeshPathStatus.PathComplete) {
            StopCoroutine(CreatePath(targetPos));
        }

        NavMeshPath agentPath = agent.path;
        agent.enabled = false;

        foreach (Vector3 node in agentPath.corners.Where(node => !path.ContainsValue(node))) {
            path.Add(path.Count, node);
        }

        index = 0;
        isCreatingPath = false;
    }

    private void RandomizeClothes() {
        MeshRenderer ren = GetComponentInChildren<MeshRenderer>();
        Material[] materials = ren.sharedMaterials;
        Material[] newMaterials = new Material[materials.Length];

        for (int i = 0; i < materials.Length; i++) {
            newMaterials[i] = new Material(materials[i]);
        }

        newMaterials[1].color = Random.ColorHSV();
        ren.materials = newMaterials;
    }

    private void CheckStuck() {
        if (path.Count <= 0 || index == -1) return;
        velMag = agent.velocity.magnitude;
        if (agent.velocity.magnitude > 0.1f) return;
        CreatePath(GetRandomPos());
    }
}