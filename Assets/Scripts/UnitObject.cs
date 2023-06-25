using UnityEngine;

public class UnitObject : MonoBehaviour {
    [SerializeField] private Unit unit;
    public Unit Unit {
        get => unit;
        set => unit = value;
    }
}