using UnityEngine;

public class BrainwaveExport : MonoBehaviour
{
    private CSVSerializer serializer = new();

    private void OnEnable()
    {

        BrainWaveBandsHandler.OnBrainwaveBandsReceived += this.ExportBrainwavesToCSV;
    }

    private void OnDisable()
    {
        BrainWaveBandsHandler.OnBrainwaveBandsReceived -= this.ExportBrainwavesToCSV;
    }

    private void ExportBrainwavesToCSV(BrainWaveBands bands)
    {
        this.serializer.DataToCSV(bands);
    }
}
