using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CSVSerializer
{
    public bool DataToCSV(BrainWaveBands data)
    {
        StringBuilder csv = new StringBuilder();

        float alpha = (float)data.alpha.Average();
        float beta = (float)data.beta.Average();
        float delta = (float)data.delta.Average();
        float gamma = (float)data.gamma.Average();
        float theta = (float)data.theta.Average();

        try
        {
            FileStream fs;
            string path = $"{Application.persistentDataPath}/brainwave-bands.csv";

            if (!File.Exists(path))
            {
                fs = File.Create(path);
                csv.AppendLine("elapsed_milliseconds;alpha;beta;delta;gamma;theta");
            }
            else
            {
                fs = File.Open(path, FileMode.Append);
            }

            csv.AppendLine($"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()};{alpha};{beta};{delta};{gamma};{theta}");

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
