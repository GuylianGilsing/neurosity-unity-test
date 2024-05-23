using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Notion.Unity;
using UnityEngine;

public class BrainWaveBandsHandler : MonoBehaviour
{
    private void OnEnable()
    {
        CrownHandler.OnBrainwaveBandsReceived += this.HandleRawBrainWaves;
    }

    private void OnDisable()
    {
        CrownHandler.OnBrainwaveBandsReceived -= this.HandleRawBrainWaves;
    }

    private void HandleRawBrainWaves(PowerByBand powerByBand)
    {
        BrainWaveBands bands = BrainwaveDecoder.decodeFromPowerByBand(powerByBand);
        Debug.Log(JsonConvert.SerializeObject(bands));
    }
}
