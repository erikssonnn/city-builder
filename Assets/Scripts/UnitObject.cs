using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour {
    [SerializeField] private Unit unit;

    public List<Vector3Int> Positions { get; set; } = new List<Vector3Int>();

    public Unit Unit {
        get => unit;
        set => unit = value;
    }

    public List<Vector3Int> GetBuildingPositions(Vector3 pos) {
        Vector3Int size = Unit.buildingSize;
        List<Vector3Int> ret = new List<Vector3Int>();

        Vector3Int roundedPos =
            new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

        int xOffset = Mathf.FloorToInt(size.x / 2f);
        int zOffset = Mathf.FloorToInt(size.z / 2f);

        for (int x = 0; x < size.x; x++) {
            for (int z = 0; z < size.z; z++) {
                Vector3Int occupiedPosition =
                    new Vector3Int(roundedPos.x - xOffset + x, roundedPos.y, roundedPos.z - zOffset + z);
                ret.Add(occupiedPosition);
            }
        }

        return ret;
    }
}