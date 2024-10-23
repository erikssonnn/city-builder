using _0_Core.Input;
using UnityEngine;

namespace _1_Game {
    public class InputController : MonoBehaviour {
        
        /// <summary>
        /// Checks for player input every tick. Check the static InputMapper keycode defines
        /// </summary>
        private void Update() {
            if (Input.GetKeyDown(InputMapper.DEBUG)) {
                
            }
        }
    }
}