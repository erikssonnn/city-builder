using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(menuName = "Custom/Occupation", fileName = "new Occupation", order = 2)]
    public class Occupation : ScriptableObject {
        public new string name;
        public int index;
    }
}