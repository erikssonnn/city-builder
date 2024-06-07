using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace erikssonn {
    [CustomEditor(typeof(ScreenOverlay))]
    public class ScreenOverlayEditor : Editor {
        SerializedProperty effects;

        void OnEnable() {
            effects = serializedObject.FindProperty("effects");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(effects, new GUIContent("Effects"), true);
            serializedObject.ApplyModifiedProperties();
        }
    }

    public class ScreenOverlay : MonoBehaviour {
        [HideInInspector] public List<Material> effects = new List<Material>();

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (effects.Count == 0) {
                Graphics.Blit(source, destination);
                Logger.Print("No effects on ScreenOverlay");
                return;
            }

            RenderTexture currentRenderTexture = source;
            RenderTexture destinationRenderTexture = RenderTexture.GetTemporary(source.width, source.height);
            for (int i = 0; i < effects.Count; i++) {
                if (effects[i] == null) {
                    Logger.Throw("Invalid material on ScreenOverlay");
                    return;
                }

                Graphics.Blit(currentRenderTexture, destinationRenderTexture, effects[i]);
                if (i < effects.Count - 1) {
                    RenderTexture temp = currentRenderTexture;
                    currentRenderTexture = destinationRenderTexture;
                    destinationRenderTexture = (i == effects.Count - 2) ? destination : RenderTexture.GetTemporary(source.width, source.height);
                    RenderTexture.ReleaseTemporary(temp);
                }
            }

            if (currentRenderTexture != destination) {
                Graphics.Blit(currentRenderTexture, destination);
            }

            RenderTexture.ReleaseTemporary(currentRenderTexture);
            RenderTexture.ReleaseTemporary(destinationRenderTexture);
        }
    }
}