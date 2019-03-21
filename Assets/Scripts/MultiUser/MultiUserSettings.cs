using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUserSettings : MonoBehaviour
{

    public static MultiUserSettings multiUserSettings;

    public bool delayStart;
    public byte maxUsers;

    public int menuScene;
    public int multiUserScene;

    private void Awake(){
        //If there are not settings, set the settings to the current
        if(MultiUserSettings.multiUserSettings == null){
            MultiUserSettings.multiUserSettings = this;
        }
        //If the current settings aren't this one, destroy them
        else{
            if(MultiUserSettings.multiUserSettings != this){
                Destroy(this.gameObject);
            }
        }
        //Don't destroy this game object by default
        DontDestroyOnLoad(this.gameObject);
    }

}
