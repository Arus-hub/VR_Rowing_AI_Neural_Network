using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;


public class CaptureDataScript : MonoBehaviour
{
    //For getting the VR controllers and headset
    public Transform headset;
    public Transform leftController;
    public Transform rightController;
    public HeadsetMotionGetter motion_getter;

    private StreamWriter writer;

    //float[] dataVals;
    //string[] dataNames;

    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        // make a log file name based on current time
      /*  DateTime startTime = DateTime.Now;
        string path = Application.persistentDataPath + "/" + startTime.ToString("yyyyMMdd_HHmm_ss") + "_data1.csv";
        writer = new StreamWriter(path, true);
        UnityEngine.Debug.Log("Writing log to: " + path);
        string header = String.Join(",", motion_getter.input_bindings);
        //print(header);
        writer.WriteLine(header);*/
    }

    // Update is called once per frame
    // use this to capture data and save to the CSV file
    void Update()
    {
       /* Dictionary<string, Vector4> values = motion_getter.outputs;
        string[] col_names = motion_getter.input_bindings;
        //string outStr = "";

        List<string> formatted_values = new List<string>();

        for (int c = 0; c < col_names.Length; c++)
        {
            //outStr += string.Format("{0:f}", values[col_names[c]]);

            Vector4 val = values[col_names[c]];
            string formatted_value = string.Format("{0:f},{1:f},{2:f},{3:f}", val.x, val.y, val.z, val.w);
            formatted_values.Add(formatted_value);
        }
        string outStr = string.Join(",", formatted_values);
        //print(outStr);
        writer.WriteLine(outStr); */
    }

    void OnDestroy()
    {
       /* writer.Close(); */
    }
}