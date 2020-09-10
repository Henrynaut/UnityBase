using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatmapSpawner : MonoBehaviour
{

    public List<string> LocationsX;
    public List<string> LocationsY;
    public List<string> LocationsZ;
    public List<string> LocationsXb;
    public List<string> LocationsYb;
    public List<string> LocationsZb;
    public GameObject heatmapSphere;
    public GameObject heatmapSphereb;
    
    public GameObject whiteboard;
    public GameObject nonWhiteboard;



    // Start is called before the first frame update
    void Start()
    {
        using(var reader = new StreamReader(@"D:\Github\UnityBase\heatmapData\combined.csv"))

        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            List<string> listC = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                listA.Add(values[3]);
                listB.Add(values[4]);
                listC.Add(values[5]);

                LocationsX = listA;
                LocationsY = listB;
                LocationsZ = listC;

            }
        }
        using(var reader = new StreamReader(@"D:\Github\UnityBase\heatmapData\combinedb.csv"))

        {
            List<string> listAb = new List<string>();
            List<string> listBb = new List<string>();
            List<string> listCb = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                listAb.Add(values[3]);
                listBb.Add(values[4]);
                listCb.Add(values[5]);

                LocationsXb = listAb;
                LocationsYb = listBb;
                LocationsZb = listCb;

            }
        }

        for (int i = 0; i < 6228; i++) {
            GameObject newSphere = (GameObject)Instantiate(heatmapSphere, new Vector3(
                float.Parse(LocationsX[i]),
                float.Parse(LocationsY[i]),
                float.Parse(LocationsZ[i])), Quaternion.Euler(Randon.Range(0.0f, 360.0f), Randon.Range(0.0f, 360.0f), Randon.Range(0.0f, 360.0f)));
            newSphere.transform.parent = whiteboard.transform;
         }

        for (int i = 0; i < 9221; i++) {
            GameObject newSphereb = (GameObject)Instantiate(heatmapSphereb, new Vector3(
                float.Parse(LocationsXb[i]),
                float.Parse(LocationsYb[i]),
                float.Parse(LocationsZb[i])), Quaternion.Euler(Randon.Range(0.0f, 360.0f), Randon.Range(0.0f, 360.0f), Randon.Range(0.0f, 360.0f)));
            newSphereb.transform.parent = nonWhiteboard.transform;
         }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}