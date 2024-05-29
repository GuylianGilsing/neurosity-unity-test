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

    private float[] alphaWaves = new float[10];
    private float[] betaWaves = new float[10];
    private float[] deltaWaves = new float[10];
    private float[] gammaWaves = new float[10];
    private float[] thetaWaves = new float[10];

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
        float alpha = (float)bands.alpha.Average();
        float beta = (float)bands.beta.Average();
        float delta = (float)bands.delta.Average();
        float gamma = (float)bands.gamma.Average();
        float theta = (float)bands.theta.Average();

        Debug.Log($"A: ${alpha} B: {beta} D: {delta} G: {gamma} T: {theta}");

        this.alphaChannel.color = this.GetColorFromBandRange(alpha);
        this.betaChannel.color = this.GetColorFromBandRange(beta);
        this.deltaChannel.color = this.GetColorFromBandRange(delta);
        this.gammaChannel.color = this.GetColorFromBandRange(gamma);
        this.thetaChannel.color = this.GetColorFromBandRange(theta);
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
