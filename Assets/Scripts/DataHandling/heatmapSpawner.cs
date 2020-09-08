using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatmapSpawner : MonoBehaviour
{

    public List<string> LocationsX;
    public List<string> LocationsY;
    public List<string> LocationsZ;
    public GameObject sphere;

    // Start is called before the first frame update
    void Start()
    {
        using(var reader = new StreamReader(@"D:\Github\UnityBase\heatmapData\1212_locationData.csv"))

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

                LocationsZ = listA;
                LocationsY = listB;
                LocationsZ = listC;

            }
        }

        // for (int i = 0; i = 270; i++) {
        //     Instantiate(sphere, new Vector3(i, 0, 0), Quaternion.identity);
        //  }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}