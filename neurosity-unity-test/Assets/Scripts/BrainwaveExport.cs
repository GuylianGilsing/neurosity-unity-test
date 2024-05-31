using Notion.Unity;
using UnityEngine;

public class BrainwaveExport : MonoBehaviour
{
    private CSVSerializer serializer = new();
    private Accelerometer accelerometer = new();

    private void OnEnable()
    {

        BrainWaveBandsHandler.OnBrainwaveBandsReceived += this.ExportBrainwavesToCSV;
        CrownHandler.OnBrainwaveAccelerometerReceived += this.HandleAccelerometer;
    }

    private void OnDisable()
    {
        BrainWaveBandsHandler.OnBrainwaveBandsReceived -= this.ExportBrainwavesToCSV;
        CrownHandler.OnBrainwaveAccelerometerReceived -= this.HandleAccelerometer;
    }

    private void HandleAccelerometer(Accelerometer data)
    {
        this.accelerometer = data;
    }

    private void ExportBrainwavesToCSV(BrainWaveBands bands)
    {
        this.serializer.DataToCSV(bands, this.accelerometer);
    }
}
