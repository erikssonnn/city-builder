using UnityEngine;

namespace _0_Core.Input {
    public class InputKey {
        public KeyCode KeyCode;
        public int MouseButton;

        public InputKey(KeyCode keyCode, int mouseButton = -1) {
            KeyCode = keyCode;
            MouseButton = mouseButton;
        }
    }
}