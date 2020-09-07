using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
public class NoiseTextureWindow :  EditorWindow
{

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/NoiseTextureGenerator")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        NoiseTextureWindow window = (NoiseTextureWindow)EditorWindow.GetWindow(typeof(NoiseTextureWindow));
        window.Show();
    }

    [SerializeField]
    NoiseType noiseType = 0;

    [SerializeField]
    float boxSize = 0.8f;
    [SerializeField]
    int deepness = 10;
    [SerializeField]
    int delta = 10;


    [SerializeField]
    TextureSpace textureSpace = TextureSpace.texture3D_Atlas;
    [SerializeField]
    int resolution2d = 1024;
    [SerializeField]
    int resolution3d = 256;

    private Texture2D noiseTex;

    string tPath = "Assets/WordSpaceTransitions/fading/t2";
    string tName = "Noise";
    bool saved = false;


    void OnEnable()
    {
        //restore values from Editor.Prefs
        if (EditorPrefs.HasKey("noiseType")) noiseType = (NoiseType)EditorPrefs.GetInt("noiseType");
        if (EditorPrefs.HasKey("boxSize")) boxSize = EditorPrefs.GetFloat("boxSize");
        if (EditorPrefs.HasKey("deepness")) deepness = EditorPrefs.GetInt("deepness");
        if (EditorPrefs.HasKey("delta")) delta = EditorPrefs.GetInt("delta");
        if (EditorPrefs.HasKey("textureSpace")) textureSpace = (TextureSpace)EditorPrefs.GetInt("textureSpace");
        if (EditorPrefs.HasKey("resolution2d")) resolution2d = EditorPrefs.GetInt("resolution2d");
        if (EditorPrefs.HasKey("resolution3d")) resolution3d = EditorPrefs.GetInt("resolution3d");
        if (EditorPrefs.HasKey("tName")) tName = EditorPrefs.GetString("tName");

        titleContent = new GUIContent("Generator");
        maxSize = new Vector2(4096, 4096);
        NoiseGenerator.noiseType = noiseType;
    }


    void OnGUI()
    {
        GUILayout.Label("Noise Texture Generator", EditorStyles.boldLabel);


        noiseType = (NoiseType)EditorGUILayout.EnumPopup("noise type", noiseType);

        if ((int)noiseType == 0 || (int) noiseType == 1)
        {
            boxSize = EditorGUILayout.Slider("boxSize", boxSize, 0, 1);
            deepness = EditorGUILayout.IntField("deepness", deepness);
            delta = EditorGUILayout.IntField("delta", delta);
        }
        else
        {
            EditorGUILayout.Space();
        }

        textureSpace = (TextureSpace)EditorGUILayout.EnumPopup("texture space", textureSpace);

        if((int)textureSpace==0) resolution2d = EditorGUILayout.IntSlider("texture2d size", resolution2d, 2, 2048);
        if((int)textureSpace==1) resolution3d = EditorGUILayout.IntSlider("texture3d size", resolution3d, 2, 256);
        //limit atlas textures to squares
        int sqrt = Mathf.RoundToInt(Mathf.Pow(resolution3d,0.25f));
        resolution3d = sqrt*sqrt*sqrt*sqrt;
        //
       if (GUI.changed)
        {
            NoiseGenerator.noiseType = noiseType;
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate Texture", GUILayout.Width(150)))
        {
            if((int)textureSpace==0) noiseTex = NoiseGenerator.Generate2D(boxSize, deepness, delta, resolution2d);
            if ((int)textureSpace == 1) noiseTex = NoiseGenerator.Generate3DAtlas(boxSize, deepness, delta, resolution3d);
            saved = false;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        Rect rect = GUILayoutUtility.GetLastRect();
        Vector2 size = rect.size;
        //Debug.Log(rect.ToString());

        if (noiseTex)
        {
            rect.position += new Vector2(155, rect.height+10);
            if (rect.width - 159 > 0)
            {
                rect.width = Mathf.Min(rect.width - 159, noiseTex.width);
                rect.height = rect.width * noiseTex.height / noiseTex.width;
                EditorGUI.DrawPreviewTexture(rect, noiseTex, null, ScaleMode.ScaleToFit);
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Save Texture", GUILayout.Width(120), GUILayout.Height(25)))
            {
                //encode creation parameters into the texture name
                string[] tNameParts = tName.Split('_');
                int nParts = tNameParts.Length;
                tName = tNameParts[0] + "_" + textureSpace.ToString().Replace("texture", "") + "_" + noiseType.ToString();
                if ((int)noiseType != 2) tName += "_" + boxSize.ToString() + "_" + deepness.ToString("0") + "_" + delta.ToString("0");
                tName += "_";
                if (nParts > 1) tName += tNameParts[nParts - 1];
                //

                string path = EditorUtility.SaveFilePanel("Save Noise Texture", tPath, tName + ".png", "png");

                if (path.Length > 0)
                {
                    byte[] bytes = noiseTex.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
                    tName = Path.GetFileNameWithoutExtension(path);
                    tPath = Path.GetDirectoryName(path);
                    saved = true;
                    if (path.Contains(Application.dataPath)) //reimport if saved within the Assets folder
                    {
                        AssetDatabase.Refresh();
                        string assetPath = path.Replace(Application.dataPath, "Assets");
                        TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                        A.textureCompression = TextureImporterCompression.Uncompressed;

                        if (noiseType == NoiseType.pixel) A.filterMode = FilterMode.Point;
                        A.maxTextureSize = 8192;
                        A.mipmapEnabled = false;
                        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                        tName = Path.GetFileNameWithoutExtension(path);
                    }
                }
            }

            GUILayout.Space(20);
            GUI.skin.label.wordWrap = true;
            if (saved) GUILayout.Label(new GUIContent(tName+".png"), GUILayout.Width(120), GUILayout.ExpandHeight(true));
        }
    }

    void OnDestroy()
    {
        //Save settings to Editor.Prefs
        EditorPrefs.SetFloat("boxSize", boxSize);
        EditorPrefs.SetInt("noiseType", (int)noiseType);
        EditorPrefs.SetInt("deepness", deepness);
        EditorPrefs.SetInt("delta", delta);
        EditorPrefs.SetInt("textureSpace", (int)textureSpace);
        EditorPrefs.SetInt("resolution3d", resolution3d);
        EditorPrefs.SetInt("resolution2d", resolution2d);
        EditorPrefs.SetString("tName", tName);
        //
    }
}
