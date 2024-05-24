using System;
using Notion.Unity;
using UnityEngine;

public class BrainWaveBandsHandler : MonoBehaviour
{
    public static event Action<BrainWaveBands> OnBrainwaveBandsReceived;

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

        OnBrainwaveBandsReceived.Invoke(bands);
    }
}
