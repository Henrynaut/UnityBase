using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(Dropdown))]
public class GetBuildScenes : MonoBehaviour {
    
    private Dropdown sceneDropdown;

    private void Start()
    {
        sceneDropdown = GetComponent<Dropdown>();
        FindAllBuildScenes();
        //sceneDropdown.onValueChanged.AddListener(delegate { SwitchScene(sceneDropdown.value); });
    }

    void FindAllBuildScenes()
    {
    #if UNITY_EDITOR
        List<string> sceneNames = new List<string>();
        foreach (EditorBuildSettingsScene sc in EditorBuildSettings.scenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(sc.path);
            Debug.Log(sceneName);
            sceneName = sceneName.Replace("_", " ");
            char[] letters = sceneName.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            sceneName = new string(letters);
            if (sc.enabled)  sceneNames.Add(sceneName);


        }


        /*int n = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < n; i++)
        {
            string sceneName = SceneManager.GetSceneByBuildIndex(i).name;
            Debug.Log(i.ToString()+" "+sceneName);
            sceneName = sceneName.Replace("_", " ");
            char[] letters = sceneName.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            sceneName = new string(letters);
            sceneNames.Add(sceneName);
        }
        */
        sceneDropdown.ClearOptions();
        sceneDropdown.AddOptions(sceneNames);
    #endif
    }
    private void OnEnable()
    {
        sceneDropdown = GetComponent<Dropdown>();
        FindAllBuildScenes();
    }
}
