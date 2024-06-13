using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seassonMaterialColorChangerURP : MonoBehaviour
{
    [Header("General setting")]
    public float colorSwitchSpeed;

    [Header("Colors")]
    public MeshRenderer meshWithMaterial;
    private Material material;
    public string[] colorNames;
    public Color[] springTreeShaderPropertiesColor;
    public Color[] summerTreeShaderPropertiesColor;
    public Color[] autumnTreeShaderPropertiesColor;
    public Color[] winterTreeShaderPropertiesColor;

    private SeasonsSystemURP.seasons materialsColorsSeason;

    private void Start()
    {
        material = meshWithMaterial.material;
    }

    void Update()
    {
        if(materialsColorsSeason != SeasonsSystemURP.season) { setColorsToNewSeason(); }
    }

    private void setColorsToNewSeason()
    {
        Color currentColor;
        Color lerpedColor;
        bool isColorTransitionFinished = false;

        switch (SeasonsSystemURP.season)
        {
            case SeasonsSystemURP.seasons.spring:
                for (int i = 0; i < colorNames.Length; i++)
                {
                    currentColor = material.GetColor(colorNames[i]);
                    (lerpedColor, isColorTransitionFinished) = lerpMaterialColors(currentColor, springTreeShaderPropertiesColor[i]);
                    if (!isColorTransitionFinished) { material.SetColor(colorNames[i], lerpedColor); }
                }
                break;
            case SeasonsSystemURP.seasons.summer:
                for (int i = 0; i < colorNames.Length; i++)
                {
                    currentColor = material.GetColor(colorNames[i]);
                    (lerpedColor, isColorTransitionFinished) = lerpMaterialColors(currentColor, summerTreeShaderPropertiesColor[i]);
                    if (!isColorTransitionFinished) { material.SetColor(colorNames[i], lerpedColor); }
                }
                break;
            case SeasonsSystemURP.seasons.autumn:
                for (int i = 0; i < colorNames.Length; i++)
                {
                    currentColor = material.GetColor(colorNames[i]);
                    (lerpedColor, isColorTransitionFinished) = lerpMaterialColors(currentColor, autumnTreeShaderPropertiesColor[i]);
                    if (!isColorTransitionFinished) { material.SetColor(colorNames[i], lerpedColor); }
                }
                break;
            case SeasonsSystemURP.seasons.winter:
                for (int i = 0; i < colorNames.Length; i++)
                {
                    currentColor = material.GetColor(colorNames[i]);
                    (lerpedColor, isColorTransitionFinished) = lerpMaterialColors(currentColor, winterTreeShaderPropertiesColor[i]);
                    if (!isColorTransitionFinished) { material.SetColor(colorNames[i], lerpedColor); }
                }
                break;
        }

        if (isColorTransitionFinished) { materialsColorsSeason = SeasonsSystemURP.season; Debug.Log("Season color switch complete"); }
    }

    private (Color, bool) lerpMaterialColors(Color previousColor, Color newColor)
    {
        Color lerpedColor = Color.Lerp(previousColor, newColor, Time.deltaTime * colorSwitchSpeed);

        bool isSimilar = false;
        float colorSimilarity = (lerpedColor.r + lerpedColor.g + lerpedColor.b) - (newColor.r + newColor.g + newColor.b);
        if (colorSimilarity > 0)
        {
            isSimilar = colorSimilarity < 0.02f;
        }
        else
        {
            isSimilar = colorSimilarity > -0.02f;
        }

        return (lerpedColor, isSimilar);
    }
}
