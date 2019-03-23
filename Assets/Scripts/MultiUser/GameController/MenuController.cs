using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    //Singleton
    public static MenuController MC;

    void Awake(){
        MC = this;
        //Ensures the game object isn't destroyed when opening a new scene
        // DontDestroyOnLoad(this.gameObject);
    }

    public void OnClickCharacterPick(int whichCharacter){
        //Check to see if UserInfo Singleton exists
        if(UserInfo.UI != null){
            UserInfo.UI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }
}
