using UnityEngine;

namespace _0_Core.Input {
    public abstract class InputMapper {
        public static InputKey BUILDING_1 = new InputKey(KeyCode.Alpha1);
        public static InputKey CANCEL_BUILDING = new InputKey(KeyCode.Escape, 1);
    }
}