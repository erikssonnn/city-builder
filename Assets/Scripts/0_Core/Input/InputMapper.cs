using UnityEngine;

namespace _0_Core.Input {
    public abstract class InputMapper {
        #region BUILDINGS
        
        public static InputKey BUILDING_1 = new InputKey(KeyCode.Alpha1);
        public static InputKey CANCEL_BUILDING = new InputKey(KeyCode.Escape, 1);
        public static InputKey PLACE_BUILDING = new InputKey(KeyCode.Joystick1Button19, 0); // Joystick1Button19 is used as "not valid"
        
        #endregion
    }
}