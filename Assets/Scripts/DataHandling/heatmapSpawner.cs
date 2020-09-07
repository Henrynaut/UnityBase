using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatmapSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        using(var reader = new StreamReader(@"C:\test.csv"))
        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                listA.Add(values[0]);
                listB.Add(values[1]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}