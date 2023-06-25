using UnityEngine;

public enum UnitType { BUILDING, UNIT}
public enum BuildingType { DEFAULT }

[CreateAssetMenu(menuName = "Custom/Unit", fileName = "new Unit", order = 0)]
public class Unit : ScriptableObject {
    [Header("MAIN: ")]
    public int index;
    public new string name;
    public string description;
    public Sprite icon;
    public UnitType unitType;
    
    [Header("BUILDING: ")]
    public BuildingType buildingType;
    public Vector3Int buildingSize;
    public GameObject buildingPrefab;
}
