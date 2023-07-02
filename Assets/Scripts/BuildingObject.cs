using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class BuildingObject : MonoBehaviour {
    [SerializeField] private Building building;
    public List<Vector3Int> Positions { get; } = new List<Vector3Int>();
    public List<UnitController> AssignedUnits { get; } = new List<UnitController>();

    public Building Building {
        get => building;
        set => building = value;
    }
    
    private void Start() {
        if (building == null) {
            throw new System.Exception("Building is null!");
        }
    }

    public void AssignUnit(UnitController unit) {
        if (AssignedUnits.Count + 1 > building.capacity) return;
        AssignedUnits.Add(unit);
    }

    public void RemoveUnit() {
        if (AssignedUnits.Count <= 0) return;
        AssignedUnits.RemoveAt(0);
    }

    public List<Vector3Int> GetBuildingPositions(Vector3 pos, Quaternion rotation) {
        Vector3Int size = Building.size;
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