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
    //             alpha: this.GetRandomBrainWaveBandRanges().Average(),
    //             beta: this.GetRandomBrainWaveBandRanges().Average(),
    //             delta: this.GetRandomBrainWaveBandRanges().Average(),
    //             gamma: this.GetRandomBrainWaveBandRanges().Average(),
    //             theta: this.GetRandomBrainWaveBandRanges().Average()
    //         );

    //         this.counter = 0;
    //         this.HandleUIUpdates(bands);
    //     }
    // }

    private float[] GetRandomBrainWaveBandRanges()
    {
        return new float[8] {
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
            UnityEngine.Random.Range(0f, 250f),
        };
    }
    #endregion

    private void HandleUIUpdates(BrainWaveBands bands)
    {
        Debug.Log($"A: {bands.alpha} B: {bands.beta} D: {bands.delta} G: {bands.gamma} T: {bands.theta}");

        this.alphaChannel.color = this.GetColorFromBandRange(bands.alpha);
        this.betaChannel.color = this.GetColorFromBandRange(bands.beta);
        this.deltaChannel.color = this.GetColorFromBandRange(bands.delta);
        this.gammaChannel.color = this.GetColorFromBandRange(bands.gamma);
        this.thetaChannel.color = this.GetColorFromBandRange(bands.theta);
    }

    private Color GetColorFromBandRange(float hertz)
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
            float rangeMin = this.hertRange / colors.Length * i;
            float rangeMax = this.hertRange / colors.Length * (i + 1);

            if (hertz >= rangeMin && hertz < rangeMax)
            {
                return colors[i];
            }
        }

        return Color.black;
    }
}
