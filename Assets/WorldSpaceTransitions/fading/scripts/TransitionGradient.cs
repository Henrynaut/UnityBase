//Script to generate the transition gradient textures and save them in .png format inside the Asset folder.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    public class TransitionGradient : MonoBehaviour
    {

        public static TransitionGradient instance;

        public Gradient gradient;
        [HideInInspector]
        public string texturePath = "WorldSpaceTransitions/fading/textures/";
        [HideInInspector]
        public string filename = "_gradient";
        [HideInInspector]
        public bool textureChanged = false; //show save button, after texture got changed

        private bool _textureChanged = false;

        public static Texture2D texture;

        private bool processing = false;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        void Start()
        {
            UpdateCurvesTexture();
            if (!FadingTransition.instance) return;
            if (FadingTransition.instance.useDynamicTexture)
            {
                Shader.SetGlobalTexture("_TransitionGradient", texture);
            }
        }

        void UpdateCurvesTexture()
        {
            processing = true;
            Texture2D oldTexture = texture;
            if (texture == null)
            {
                texture = new Texture2D(256, 2, TextureFormat.RGB24, false, true);
                texture.wrapMode = TextureWrapMode.Clamp;
                //textureChanged = false;
            }
            for (int i = 0; i < 256; i++)
            {
                float x = i * 1.0f / 255;
                Color col = new Color();
                float aCh;

                if (gradient != null) col = gradient.Evaluate(x);
                aCh = col.a;
                //col.a = 1;

                texture.SetPixel(i, 0, col);
                texture.SetPixel(i, 1, new Color(aCh, aCh, aCh));
                if (_textureChanged||oldTexture==null) continue;
                if (oldTexture.GetPixel(i, 0) != col) _textureChanged = true;
                if (oldTexture.GetPixel(i, 0) != new Color(aCh, aCh, aCh)) _textureChanged = true;

            }

            texture.Apply();
            processing = false;
            //SaveTexture();
        }

        public void SaveTexture(string _path)
        {
            processing = true;
#if UNITY_EDITOR
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(_path, bytes);
            filename = Path.GetFileNameWithoutExtension(_path);
            if (_path.Contains(Application.dataPath)) //reimport if saved within the Assets folder
            {
                AssetDatabase.Refresh();
                string assetPath = _path.Replace(Application.dataPath, "Assets");
                TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                texturePath = Path.GetDirectoryName(assetPath);
                A.textureCompression = TextureImporterCompression.Uncompressed;

                A.filterMode = FilterMode.Point;
                A.wrapMode = TextureWrapMode.Clamp;
                A.mipmapEnabled = false;
                A.SaveAndReimport();
                filename = Path.GetFileNameWithoutExtension(_path);
            }
#endif
            _textureChanged = false;
            textureChanged = false;
            processing = false;
        }


#if UNITY_EDITOR
        void OnValidate()
        {
        if (FadingTransition.instance == null||processing) return;
            if (FadingTransition.instance.useDynamicTexture)
                UpdateCurvesTexture();
            Shader.SetGlobalTexture("_TransitionGradient", texture);
        }

        void Update()
        {
            textureChanged = _textureChanged;
        }
#endif
    }

}