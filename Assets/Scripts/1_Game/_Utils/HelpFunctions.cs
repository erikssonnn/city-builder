using UnityEngine;
using Vector3Int = _0_Core.Class.Vector3Int;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game._HelpFunctions {
    /// <summary>
    /// General help functions. For example when UnityEngine is needed for a conversion but cant be used in the original class
    /// </summary>
    public struct HelpFunctions {
        #region Vector3

        public static Vector3[] Vector3IntToVector3(Vector3Int[] input) {
            Vector3[] val = new Vector3[input.Length];
            for (int i = 0; i < input.Length; i++) {
                val[i] = new Vector3(input[i].x, input[i].y, input[i].z);
            }

            return val;
        }

        public static Vector3Int[] Vector3ToVector3Int(Vector3[] input) {
            Vector3Int[] val = new Vector3Int[input.Length];
            for (int i = 0; i < input.Length; i++) {
                val[i] = new Vector3Int((int)input[i].x, (int)input[i].y, (int)input[i].z);
            }

            return val;
        }

        public static Vector3 Vector3IntToVector3(Vector3Int input) => new Vector3(input.x, input.y, input.z);
        public static Vector3Int Vector3ToVector3Int(Vector3 input) => new Vector3Int((int)input.x, (int)input.y, (int)input.z);

        #endregion

        #region Vector2

        public static Vector2[] Vector2IntToVector2(Vector2Int[] input) {
            Vector2[] val = new Vector2[input.Length];
            for (int i = 0; i < input.Length; i++) {
                val[i] = new Vector2(input[i].x, input[i].y);
            }

            return val;
        }

        public static Vector2Int[] Vector2ToVector2Int(Vector2[] input) {
            Vector2Int[] val = new Vector2Int[input.Length];
            for (int i = 0; i < input.Length; i++) {
                val[i] = new Vector2Int((int)input[i].x, (int)input[i].y);
            }

            return val;
        }

        public static Vector2 Vector2IntToVector2(Vector2Int input) => new Vector2(input.x, input.y);
        public static Vector2Int Vector2ToVector2Int(Vector2 input) => new Vector2Int((int)input.x, (int)input.y);

        #endregion
    }
}