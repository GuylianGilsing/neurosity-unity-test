using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColourChanger : MonoBehaviour
{
    [SerializeField] Material planeMaterial;
    [SerializeField] Material cubeMaterial;
    [SerializeField] Material orbMaterial;
    [SerializeField] Material cylinderMaterial;

    [SerializeField] private Slider _alphaSlider;
    [SerializeField] private Slider _betaSlider;
    [SerializeField] private Slider _deltaSlider;
    [SerializeField] private Slider _gammaSlider;
    [SerializeField] private Slider _thetaSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        planeMaterial.color = new Color(_gammaSlider.value, _gammaSlider.value, _gammaSlider.value);
        cubeMaterial.color = new Color(_alphaSlider.value, _thetaSlider.value, 0);
        orbMaterial.color = new Color(0, _betaSlider.value, _thetaSlider.value);
        cylinderMaterial.color = new Color(_thetaSlider.value, 0, _deltaSlider.value);
    }
}