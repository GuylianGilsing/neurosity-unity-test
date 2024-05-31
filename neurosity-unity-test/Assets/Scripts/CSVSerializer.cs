using System;
using System.IO;
using System.Text;
using Notion.Unity;
using UnityEngine;

public class CSVSerializer
{
    public bool DataToCSV(BrainWaveBands data, Accelerometer accelerometer)
    {
        StringBuilder csv = new StringBuilder();

        try
        {
            FileStream fs;
            string path = $"{Application.persistentDataPath}/brainwave-bands.csv";

            if (!File.Exists(path))
            {
                fs = File.Create(path);
                csv.AppendLine("elapsed_milliseconds;alpha;beta;delta;gamma;theta;ac_acceleration;ac_inclination;ac_orientation;ac_pitch;ac_roll;ac_x;ac_y;ac_z");
            }
            else
            {
                fs = File.Open(path, FileMode.Append);
            }

            csv.AppendLine($"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()};{data.alpha};{data.beta};{data.delta};{data.gamma};{data.theta};{accelerometer.Acceleration};{accelerometer.Inclination};{accelerometer.Orientation};{accelerometer.Pitch};{accelerometer.Roll};{accelerometer.X};{accelerometer.Y};{accelerometer.Z}");

            fs.Write(new UTF8Encoding(true).GetBytes(csv.ToString()));
            fs.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }

        return true;
    }
}
