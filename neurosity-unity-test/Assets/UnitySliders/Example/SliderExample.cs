using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliderExample : MonoBehaviour
{
    // Start is called before the first frame update
    UnitySliders slider;
    Material material;

    void Start()
    {
        slider = GetComponent<UnitySliders>();
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider?.IsConnected() == true)
        {
            material.color = new Color(slider.GetSliderValue01(UnitySliders.SliderColors.Red),
                                        slider.GetSliderValue01(UnitySliders.SliderColors.Green),
                                        slider.GetSliderValue01(UnitySliders.SliderColors.Blue));

            this.transform.localScale = new Vector3(1 + slider.GetSliderValue01(UnitySliders.SliderColors.Orange), 1 + slider.GetSliderValue01(UnitySliders.SliderColors.Yellow), 1);
        }
    }
}
