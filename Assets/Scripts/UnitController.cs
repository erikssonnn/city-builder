using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class UnitController : MonoBehaviour {
    private NavMeshAgent agent = null;
    private Vector3 targetPosition = Vector3.zero;
    public float velMag = 0;

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(targetPosition, Vector3.one);
    }
    
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        RandomizeClothes();
        SetTargetPosition();
    }

    private void SetTargetPosition() {
        targetPosition = new Vector3(Random.Range(-25f, 25f), 0, Random.Range(-25f, 25f));
        agent.SetDestination(targetPosition);
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
        if (targetPosition == Vector3.zero) return;
        velMag = agent.velocity.magnitude;
        if (agent.velocity.magnitude > 0.1f) return;
        SetTargetPosition();
    }

    private void Update() {
        CheckStuck();
        if (targetPosition == Vector3.zero) return;
        float dist = Vector3.Distance(transform.position, targetPosition);
        if (dist > 0.5f) return;
        SetTargetPosition();
    }
}