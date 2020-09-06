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
    private string quatW;
    private string quatX;
    private string quatY;
    private string quatZ;
    private bool logDataBool;
    public float nextLogTime;
    public string participantID;


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
        quatW = avatarCamera.transform.rotation.w.ToString();
        quatX = avatarCamera.transform.rotation.x.ToString();
        quatY = avatarCamera.transform.rotation.y.ToString();
        quatZ = avatarCamera.transform.rotation.z.ToString();

        //Only record data every 0.25 seconds if logDataBool is true
        if(logDataBool && participantID.Length == 4)
        {
            intID++;                                    //Increment ID by 1
            ID = intID.ToString();                      //Convert intID to a string
            addRecord(participantID, ID, currentTime, X, Y, Z, quatW, quatX, quatY, quatZ, "heatmapData/avatarData.csv");
            logDataBool = false;
            nextLogTime = Time.time + 0.25f;
        }
        //enable logDataBool if time has reached the nextLogTime
        else if (Time.time >= nextLogTime)
        {
            logDataBool = true;
        }

    }

    public static void addRecord(string participantID,
                                 string ID,
                                 string time,
                                 string X,
                                 string Y,
                                 string Z,
                                 string quatW,
                                 string quatX,
                                 string quatY,
                                 string quatZ,
                                 string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
            {
                //Save data as a new line in CSV file
                file.WriteLine
                (
                    participantID + "," + ID + "," + time + "," + X + "," + Y + "," + Z
                    + "," + quatW + "," + quatX + "," + quatY + "," + quatZ
                );
            }
        }
        catch(Exception ex)
        {
            throw new ApplicationException("This program did an oopsie :", ex);
        }
    }
}
