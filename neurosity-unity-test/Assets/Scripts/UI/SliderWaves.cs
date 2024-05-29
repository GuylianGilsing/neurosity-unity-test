using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderWaves : MonoBehaviour
{
    [SerializeField] private Slider  _alphaSlider;
    [SerializeField] private Slider  _betaSlider;
    [SerializeField] private Slider  _deltaSlider;
    [SerializeField] private Slider  _gammaSlider;
    [SerializeField] private Slider  _thetaSlider;
    [SerializeField] private Image alphaChannel;
    [SerializeField] private Image betaChannel;
    [SerializeField] private Image deltaChannel;
    [SerializeField] private Image gammaChannel;
    [SerializeField] private Image thetaChannel;
    [SerializeField] private int hertRange = 250;

    DisplayBrainWaveBands waveBands;

    // Start is called before the first frame update
    void Start()
    {
        _alphaSlider.onValueChanged.AddListener((a) => {
            Debug.Log(_alphaSlider.value);
            this.alphaChannel.color = new Color(_alphaSlider.value / 200, 0, 0);
        });

        _betaSlider.onValueChanged.AddListener((b) => {
            Debug.Log(_betaSlider.value);
            this.betaChannel.color = new Color(0, _betaSlider.value / 200, 0);
        });

        _deltaSlider.onValueChanged.AddListener((d) => {
            Debug.Log(_deltaSlider.value);
            this.deltaChannel.color = new Color(0, 0, _deltaSlider.value / 200);
        });

        _gammaSlider.onValueChanged.AddListener((g) => {
            Debug.Log(_gammaSlider.value);
            this.gammaChannel.color = new Color(_gammaSlider.value / 200, 0, 0);
        });

        _thetaSlider.onValueChanged.AddListener((t) => {
            Debug.Log(_thetaSlider.value);
            this.thetaChannel.color = new Color(0, 0, _thetaSlider.value / 200);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
