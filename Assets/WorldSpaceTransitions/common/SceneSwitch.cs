using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneSwitch : MonoBehaviour {
    private static SceneSwitch switchInstance;
    public Dropdown sceneDropdown;

    public void SwitchScene(int val)
    {
        if (val == SceneManager.GetActiveScene().buildIndex) return; //toggle buttons change twice
        SceneManager.LoadSceneAsync(val);
    }
	void Awake () {
        DontDestroyOnLoad(this);
        if (switchInstance == null)
        {
            switchInstance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
	}
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            //Debug.Log("escape");
            Application.Quit();
        }
    }
}
