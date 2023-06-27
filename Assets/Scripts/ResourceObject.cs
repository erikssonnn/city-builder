using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class ResourceObject : MonoBehaviour {
    [SerializeField] private Resource resource;
    public List<Vector3Int> Positions { get; set; } = new List<Vector3Int>();

    public Resource Resource {
        get => resource;
        set => resource = value;
    }

    private void Start() {
        if (resource == null) {
            throw new System.Exception("Resource is null!");
        }
    }

    public List<Vector3Int> GetResourcePositions(Vector3 pos, Quaternion rotation) {
        Vector3Int size = Resource.size;
        List<Vector3Int> ret = new List<Vector3Int>();

        Vector3Int roundedPos =
            new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

        int xOffset = Mathf.FloorToInt(size.x / 2f);
        int zOffset = Mathf.FloorToInt(size.z / 2f);

        for (int x = 0; x < size.x; x++) {
            for (int z = 0; z < size.z; z++) {
                Vector3Int offsetPosition = new Vector3Int(x - xOffset, 0, z - zOffset);
                Vector3 rotatedOffset = rotation * offsetPosition;

                Vector3Int occupiedPosition =
                    new Vector3Int(roundedPos.x + Mathf.RoundToInt(rotatedOffset.x), roundedPos.y,
                        roundedPos.z + Mathf.RoundToInt(rotatedOffset.z));

                ret.Add(occupiedPosition);
            }
        }

        return ret;
    }
}