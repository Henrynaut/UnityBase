using UnityEngine;
using UnityEditor;

namespace WorldSpaceTransitions
{
#if UNITY_EDITOR
    [CustomEditor(typeof(TransitionGradient))]
    public class TransitionGradientEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TransitionGradient gradGenerator = (TransitionGradient)target;
            if (!gradGenerator.textureChanged) return;

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save gradient as texture:"))
            {
                string path = EditorUtility.SaveFilePanel("Save Gradient Texture", Application.dataPath + "/" + gradGenerator.texturePath, gradGenerator.filename + ".png", "png");
                if(path.Length>0)  gradGenerator.SaveTexture(path);
            }
            GUILayout.Space(10);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
#endif
}