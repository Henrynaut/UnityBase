using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{

    //Singleton
    public static UserInfo UI;

    public int mySelectedCharacter;

    public GameObject[] allCharacters;

    private void OnEnable(){
        //If User Info is empty, set it to current avatar data
        if(UserInfo.UI == null){
            UserInfo.UI = this;
        }
        //Else, if not empty, make sure it matches current avatar
        else{
            if(UserInfo.UI != this){
                Destroy(UserInfo.UI.gameObject);
                UserInfo.UI = this;
            }
        }
        //Ensures the game object isn't destroyed when opening a new scene
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start() {
        //Check to see if a player preference exists, and set character to selection
        if(PlayerPrefs.HasKey("MyCharacter")){
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");

        }

        //If not, default to character index 0
        else {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
        }
    }
}
