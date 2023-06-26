using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingObject : MonoBehaviour {
    [FormerlySerializedAs("unit")] [SerializeField] private Building building;

    public List<Vector3Int> Positions { get; set; } = new List<Vector3Int>();

    public Building Building {
        get => building;
        set => building = value;
    }

    public List<Vector3Int> GetBuildingPositions(Vector3 pos) {
        Vector3Int size = Building.buildingSize;
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