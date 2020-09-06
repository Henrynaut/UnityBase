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
    private string X;
    private string Y;
    private string Z;
    private string roll;
    private string pitch;
    private string yaw;
    private bool logDataBool;
    public float nextLogTime;


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
        logDataBool = true;
        nextLogTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        currentTime = Time.time.ToString("f6");     //Save game time in seconds as a string

        //Save position and rotation of avatrCamera as strings
        X = avatarCamera.transform.position.x.ToString();
        Y = avatarCamera.transform.position.y.ToString();
        Z = avatarCamera.transform.position.z.ToString();
        // roll = avatarCamera.transform.position.x.ToString();
        // pitch = avatarCamera.transform.position.x.ToString();
        // yaw = avatarCamera.transform.position.x.ToString();

        //Only record data every 0.25 seconds if logDataBool is true
        if(logDataBool)
        {
            intID++;                                    //Increment ID by 1
            ID = intID.ToString();                      //Convert intID to a string
            addRecord(ID, currentTime, X, Y, Z, "avatarData.csv");
            logDataBool = false;
            nextLogTime = Time.time + 0.25f;
        }
        //enable logDataBool if time has reached the nextLogTime
        else if (Time.time >= nextLogTime)
        {
            logDataBool = true;
        }

    }

    public static void addRecord(string ID,
                                 string time,
                                 string X,
                                 string Y,
                                 string Z,
                                //  string roll,
                                //  string pitch,
                                //  string yaw,
                                 string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
            {
                file.WriteLine(ID + "," + time + "," + X + "," + Y + "," + Z);
            }
        }
        catch(Exception ex)
        {
            throw new ApplicationException("This program did an oopsie :", ex);
        }
    }
}
