using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBrainWaveBands : MonoBehaviour
{
    [SerializeField] private int hertRange = 250;
    [SerializeField] private Image alphaChannel;
    [SerializeField] private Image betaChannel;
    [SerializeField] private Image deltaChannel;
    [SerializeField] private Image gammaChannel;
    [SerializeField] private Image thetaChannel;

    private void OnEnable()
    {
        BrainWaveBandsHandler.OnBrainwaveBandsReceived += this.HandleUIUpdates;
    }

    private void OnDisable()
    {
        BrainWaveBandsHandler.OnBrainwaveBandsReceived -= this.HandleUIUpdates;
    }

    #region TEST CODE (REMOVE THIS WHEN WORKING WITH REAL CROWN DATA)
    private float counter;

    // private void Update()
    // {
    //     this.counter += Time.deltaTime;

    //     // Updates every 2 seconds
    //     if (this.counter >= 2.0f)
    //     {
    //         BrainWaveBands bands = new(
    //             alpha: this.GetRandomBrainWaveBandRanges(),
    //             beta: this.GetRandomBrainWaveBandRanges(),
    //             delta: this.GetRandomBrainWaveBandRanges(),
    //             gamma: this.GetRandomBrainWaveBandRanges(),
    //             theta: this.GetRandomBrainWaveBandRanges()
    //         );

    //         this.counter = 0;
    //         this.HandleUIUpdates(bands);
    //     }
    // }

    private decimal[] GetRandomBrainWaveBandRanges()
    {
        return new decimal[8] {
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
            Convert.ToDecimal(UnityEngine.Random.Range(0f, 250f)),
        };
    }
    #endregion

    private void HandleUIUpdates(BrainWaveBands bands)
    {
        this.alphaChannel.color = this.GetColorFromBandRange(bands.alpha.Average());
        this.betaChannel.color = this.GetColorFromBandRange(bands.beta.Average());
        this.deltaChannel.color = this.GetColorFromBandRange(bands.delta.Average());
        this.gammaChannel.color = this.GetColorFromBandRange(bands.gamma.Average());
        this.thetaChannel.color = this.GetColorFromBandRange(bands.theta.Average());
    }

    public Color GetColorFromBandRange(decimal hertz)
    {
        Color[] colors = new Color[6] {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.cyan,
        };

        // Filter out negative ranges
        if (hertz < 0)
        {
            hertz = 0;
        }

        // Check between which range the current hertz belongs
        for (int i = 0; i < colors.Length; i += 1)
        {
            decimal rangeMin = this.hertRange / colors.Length * i;
            decimal rangeMax = this.hertRange / colors.Length * (i + 1);

            if (hertz >= rangeMin && hertz < rangeMax)
            {
                return colors[i];
            }
        }

        return Color.black;
    }
}
