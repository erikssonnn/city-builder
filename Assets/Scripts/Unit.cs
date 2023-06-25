using UnityEngine;

public enum InteractType { DEFAULT }

[CreateAssetMenu(menuName = "Custom/Unit", fileName = "new Unit", order = 0)]
public class Unit : ScriptableObject {
    public int index;
    public new string name;
    public string description;
    public Sprite icon;
    public InteractType interactType;
}
