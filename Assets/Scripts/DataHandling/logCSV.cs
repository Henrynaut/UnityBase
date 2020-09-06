using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class logCSV : MonoBehaviour
{
    private AvatarSetup avatarSetup;
    public Camera avatarCamera;
    private string currentTime;
    private string ID;
    private int intID;


    // Start is called before the first frame update
    void Start()
    {
        //Get Avatar Camera
        if (avatarCamera == null)
        {
        avatarCamera = GameObject.Find("AvatarCamera").GetComponent<Camera>();
        }

        //Intialize ID to 0
        intID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        intID++;                                    //Increment ID by 1
        ID = intID.ToString();                      //Convert intID to a string
        currentTime = Time.time.ToString("f6");     //Save game time in seconds as a string

        addRecord(ID, currentTime, "56", "avatarData.csv");
    }

    public static void addRecord(string ID, string time, string age, string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
            {
                file.WriteLine(ID + "," + time + "," + age);
            }
            Debug.Log("CSV Information saved.");
        }
        catch(Exception ex)
        {
            throw new ApplicationException("This program did an oopsie :", ex);
        }
    }
}
