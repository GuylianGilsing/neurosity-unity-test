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
        Debug.Log($"{epoch.Label}");
        Debug.Log($"{epoch.Data.ToString()}");
    }
}
