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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        addRecord("124", "Mercy", "56", "cake.txt");
    }

    public static void addRecord(string ID, string name, string age, string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
            {
                file.WriteLine(ID + "," + name + "," + age);
            }
            Debug.Log("CSV Information saved.");
        }
        catch(Exception ex)
        {
            throw new ApplicationException("This program did an oopsie :" ex);
        }
    }
}
