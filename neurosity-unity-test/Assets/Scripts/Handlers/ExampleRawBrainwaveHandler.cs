using Newtonsoft.Json;
using Notion.Unity;
using UnityEngine;

public class ExampleRawBrainwaveHandler : MonoBehaviour
{
    private void OnEnable()
    {
        CrownHandler.OnRawBrainwavesReceived += this.HandleRawBrainWaves;
    }

    private void OnDisable()
    {
        CrownHandler.OnRawBrainwavesReceived -= this.HandleRawBrainWaves;
    }

    private void HandleRawBrainWaves(Epoch epoch)
    {
        RawBrainWaves brainWaves = BrainwaveDecoder.decodeFromEpoch(epoch);
        Debug.Log(JsonConvert.SerializeObject(brainWaves));
    }
}
