using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SeasonsSystemURP : MonoBehaviour
{
    public enum seasons
    {
        none,
        spring,
        summer,
        autumn,
        winter
    }

    [Header("Seasons post processing")]
    public Volume springPP;
    public Volume summerPP;
    public Volume autumnPP;
    public Volume winterPP;

    [Header("Seasons particle effects")]
    public GameObject springParticlesFolder;
    public GameObject summerParticlesFolder;
    public GameObject autumnParticlesFolder;
    public GameObject winterParticlesFolder;
    private ParticleSystem[] springParticles;
    private ParticleSystem[] summerParticles;
    private ParticleSystem[] autumnParticles;
    private ParticleSystem[] winterParticles;
    
    [Header("Time setting")]
    public float minutesOfDay;
    public int springDays;
    public int summerDays;
    public int autumnDays;
    public int winterDays;

    [Header("General setting")]
    public bool logToConsole;
    /// <summary>
    /// If unchecked, seasons have to be changed manualy via SetSeason()
    /// </summary>
    public bool autoCycleSeasons = true;
    [Range(0.05f, 1f)]
    public float seasonSwitchSpeed;
    public seasons initialSeason;

    [Header("Fog")]
    public float springFogDensity;
    public float summerFogDensity;
    public float autumnFogDensity;
    public float winterFogDensity;

    [Range(0.01f, 0.5f)]
    public float fogDensitySwitchSpeed;

    private float seasonFogDensity;

    [Header("Grass")]
    public bool changeGrassColor;
    public bool changeGrassWindSpeed;
    [Space(1f)]
    public float grassSwitchSpeed;
    public Terrain terrain;
    public GrassColor[] grassColors;
    private DetailPrototype[] detailPrototypes;
    public WindSpeed windSpeed;

    [Header("Materials")]
    public SeasonMaterial[] seasonMaterials;

    public static int dayOfMonth = 1;
    public static int days_total = 1;

    /// <summary>
    /// Get current season. Use SetSeason to change season.
    /// </summary>
    public static seasons season { get; private set; }

    private Dictionary<seasons, int> seasonDays = new Dictionary<seasons, int>();

    /// <summary>
    /// is set to true when corresponding season PP weight is 1 and is reset to false with new season.
    /// </summary>
    private bool isNewSeasonPostProcessingSet;
    private float m_minutesOfDay;
    private float dayTimer;
    private bool isGrassColorsSet;
    private bool isAllMaterialsRecolored;


    void Start()
    {
        m_minutesOfDay = minutesOfDay * 60;
        dayTimer = m_minutesOfDay;

        season = initialSeason;
        onNewSeasonActions();
        if (changeGrassColor || changeGrassWindSpeed) { detailPrototypes = terrain.terrainData.detailPrototypes; }

        findParticlesInFolders();
        init_seasonDays();
    }


    void Update()
    {
        if (!autoCycleSeasons)
        {
            enabled = false;
            return;
        }

        dayCountdown();
        seasonPostProcessing();
        checkAndSetFogDensity();
        recolorMaterials();
        if (changeGrassColor) { recolorGrass(); }
    }

    public void SetSeason(seasons season)
    {
        SeasonsSystemURP.season = season;
        onNewSeasonActions();
    }

    /// <summary>
    /// Immediately set season. Usefull for initial / fast season swap
    /// </summary>
    /// <param name="season">Season to set</param>
    public void SetSeasonInstanly(seasons season)
    {
        SeasonsSystemURP.season = season;
        onNewSeasonActions();

        // Post processing
        springPP.weight = 0;
        summerPP.weight = 0;
        autumnPP.weight = 0;
        winterPP.weight = 0;
        var postProcessing = GetSeasonPostProcessing(season);
        postProcessing.weight = 1;

        // Particles
        playParticleEffects(springParticles, false);
        playParticleEffects(summerParticles, false);
        playParticleEffects(autumnParticles, false);
        playParticleEffects(winterParticles, false);
        playParticleEffects(GetSeasonParticles(season), true);

        // Fog
        RenderSettings.fogDensity = seasonFogDensity;

        // Grass color
        for (int i = 0; i < grassColors.Length; i++)
        {
            detailPrototypes[i].dryColor = getSeasonalGrassColor(grassColors[i], false);
            detailPrototypes[i].healthyColor = getSeasonalGrassColor(grassColors[i], true);
        }
        terrain.terrainData.detailPrototypes = detailPrototypes;
    }

    private void findParticlesInFolders()
    {
        springParticles = springParticlesFolder.GetComponentsInChildren<ParticleSystem>();
        summerParticles = summerParticlesFolder.GetComponentsInChildren<ParticleSystem>();
        autumnParticles = autumnParticlesFolder.GetComponentsInChildren<ParticleSystem>();
        winterParticles = winterParticlesFolder.GetComponentsInChildren<ParticleSystem>();
    }

    private void init_seasonDays()
    {
        seasonDays.Add(seasons.spring, springDays);
        seasonDays.Add(seasons.summer, summerDays);
        seasonDays.Add(seasons.autumn, autumnDays);
        seasonDays.Add(seasons.winter, winterDays);
    }

    private void dayCountdown()
    {
        dayTimer -= Time.deltaTime;
        if(dayTimer <= 0)
        {
            startNewDay();
            checkSeason();
        }
    }

    private void checkSeason()
    {
        if(dayOfMonth > seasonDays[season]) { season = nextSeason(); onNewSeasonActions(); }
    }

    private seasons nextSeason()
    {
        if (logToConsole) { Debug.Log("Season system: end of season " + season.ToString()); }

        switch (season)
        {
            case seasons.spring:
                return seasons.summer;
            case seasons.summer:
                return seasons.autumn;
            case seasons.autumn:
                return seasons.winter;
            case seasons.winter:
                return seasons.spring;
        }

        return seasons.none;
    }

    private void onNewSeasonActions()
    {
        dayOfMonth = 0;
        isNewSeasonPostProcessingSet = false;
        isGrassColorsSet = false;
        isAllMaterialsRecolored = false;

        if (changeGrassWindSpeed) { changeGrassSpeed(); }

        switch (season)
        {
            case seasons.spring:
                seasonFogDensity = springFogDensity;
                break;
            case seasons.summer:
                seasonFogDensity = summerFogDensity;
                break;
            case seasons.autumn:
                seasonFogDensity = autumnFogDensity;
                break;
            case seasons.winter:
                seasonFogDensity = winterFogDensity;
                break;
        }
    }

    private void startNewDay()
    {
        if (logToConsole) { Debug.Log("Season system: new day " + dayOfMonth.ToString()); }

        dayOfMonth++;
        days_total++;
        dayTimer = m_minutesOfDay;

        seasonEffects();
    }

    private void seasonEffects()
    {
        switch (season)
        {
            case seasons.spring:
                winterEffects(false);
                playParticleEffects(springParticles, true);
                break;
            case seasons.summer:
                playParticleEffects(springParticles, false);
                playParticleEffects(summerParticles, true);
                break;
            case seasons.autumn:
                playParticleEffects(summerParticles, false);
                playParticleEffects(autumnParticles, true);
                break;
            case seasons.winter:
                autumnEffects(false);
                winterEffects(true);
                break;
            default:
                break;
        }
    }

    private ParticleSystem[] GetSeasonParticles(seasons season)
    {
        switch (season)
        {
            case seasons.spring:
                return springParticles;
            case seasons.summer:
                return summerParticles;
            case seasons.autumn:
                return autumnParticles;
            case seasons.winter:
                return winterParticles;
        }

        return new[] {new ParticleSystem() };
    }

    private void playParticleEffects(ParticleSystem[] effects, bool play)
    {
        if (effects == null) { return; }

        foreach (var effect in effects)
        {
            if (play) { effect.Play(); }
            else { effect.Stop(); }
        }
    }

    private void autumnEffects(bool play)
    {
        if (autumnParticles == null) { return; }

        foreach (var autumnParticle in autumnParticles)
        {
            if (play) { autumnParticle.Play(); }
            else { autumnParticle.Stop(); }
        }
    }

    private void winterEffects(bool play)
    {
        if (winterParticles == null) { return; }

        foreach (var winterParticle in winterParticles)
        {
            if (play) { winterParticle.Play(); }
            else { winterParticle.Stop(); }
        }
    }

    #region Post processing

    private void seasonPostProcessing()
    {
        if (isNewSeasonPostProcessingSet) { return; }

        switch (season)
        {
            case seasons.spring:
                isNewSeasonPostProcessingSet = setPostProcessing(false, winterPP);
                isNewSeasonPostProcessingSet = setPostProcessing(true, springPP);
                break;
            case seasons.summer:
                isNewSeasonPostProcessingSet = setPostProcessing(false, springPP);
                isNewSeasonPostProcessingSet = setPostProcessing(true, summerPP);
                break;
            case seasons.autumn:
                isNewSeasonPostProcessingSet = setPostProcessing(false, summerPP);
                isNewSeasonPostProcessingSet = setPostProcessing(true, autumnPP);
                break;
            case seasons.winter:
                isNewSeasonPostProcessingSet = setPostProcessing(false, autumnPP);
                isNewSeasonPostProcessingSet = setPostProcessing(true, winterPP);
                break;
            default:
                break;
        }
    }

    private Volume GetSeasonPostProcessing(seasons season)
    {
        switch (season)
        {
            case seasons.spring:
                return springPP;
            case seasons.summer:
                return summerPP;
            case seasons.autumn:
                return autumnPP;
            case seasons.winter:
                return winterPP;
        }

        return new Volume();
    }

    private bool setPostProcessing(bool set, Volume postProcessing)
    {
        if(postProcessing == null) { Debug.LogError(season.ToString() + " or previous season post processing is not set!"); return true; }

        if (set)
        {
            if (postProcessing.weight < 1) { postProcessing.weight += Time.deltaTime * seasonSwitchSpeed; return false; }
            else { postProcessing.weight = 1; return true; }
        }
        else
        {
            if (postProcessing.weight > 0.1) { postProcessing.weight -= Time.deltaTime * seasonSwitchSpeed; return false; }
            else { postProcessing.weight = 0; return true; }
        }
    }

    #endregion

    #region Fog
    private void checkAndSetFogDensity()
    {
        if (RenderSettings.fogDensity < seasonFogDensity) 
        { 
            RenderSettings.fogDensity += (Time.deltaTime * fogDensitySwitchSpeed) / 1000;
        }

        if (RenderSettings.fogDensity > seasonFogDensity) 
        { 
            RenderSettings.fogDensity -= (Time.deltaTime * fogDensitySwitchSpeed) / 1000;
        }
    }
    #endregion

    #region Grass

    private void recolorGrass()
    {
        if (isGrassColorsSet) { return; }

        bool isDryRecolorFinished = false;
        bool isHealthyRecolorFinished = false;
        Color lerpedColor;
        for (int i = 0; i < grassColors.Length; i++)
        {
            int grassPositionInTerrainData = grassColors[i].terrainPosition;
            (lerpedColor, isDryRecolorFinished) = lerpMaterialColors(detailPrototypes[grassPositionInTerrainData].dryColor, getSeasonalGrassColor(grassColors[i], false));
            if (!isDryRecolorFinished)
            {
                detailPrototypes[i].dryColor = lerpedColor;
            }

            (lerpedColor, isHealthyRecolorFinished) = lerpMaterialColors(detailPrototypes[grassPositionInTerrainData].healthyColor, getSeasonalGrassColor(grassColors[i], true));
            if (!isHealthyRecolorFinished)
            {
                detailPrototypes[i].healthyColor = lerpedColor;
            }
        }

        terrain.terrainData.detailPrototypes = detailPrototypes;

        if(isHealthyRecolorFinished && isDryRecolorFinished) { isGrassColorsSet = true; }
    }

    private void changeGrassSpeed()
    {
        switch (season)
        {
            case seasons.spring:
                terrain.terrainData.wavingGrassSpeed = windSpeed.spring;
                terrain.terrainData.wavingGrassStrength = windSpeed.spring;
                break;
            case seasons.summer:
                terrain.terrainData.wavingGrassSpeed = windSpeed.summer;
                terrain.terrainData.wavingGrassStrength = windSpeed.summer;
                break;
            case seasons.autumn:
                terrain.terrainData.wavingGrassSpeed = windSpeed.autumn;
                terrain.terrainData.wavingGrassStrength = windSpeed.autumn;
                break;
            case seasons.winter:
                terrain.terrainData.wavingGrassSpeed = windSpeed.winter;
                terrain.terrainData.wavingGrassStrength = windSpeed.winter;
                break;
        }
    }

    private Color getSeasonalGrassColor(GrassColor grassColor, bool healthy)
    {
        switch (season)
        {
            case seasons.spring:
                return healthy ? grassColor.springHealtyColor : grassColor.springDryColor;
            case seasons.summer:
                return healthy ? grassColor.summerHealtyColor : grassColor.summerDryColor;
            case seasons.autumn:
                return healthy ? grassColor.autumnHealtyColor : grassColor.autumnDryColor;
            case seasons.winter:
                return healthy ? grassColor.winterHealtyColor : grassColor.winterDryColor;
            default:
                return new Color();
        }
    }

    #endregion

    #region Materials recolor

    private void recolorMaterials()
    {
        if(seasonMaterials == null) { return; }

        if (isAllMaterialsRecolored) { return; }

        bool isMaterialRecolored;
        Color lerpedColor;
        for (int i = 0; i < seasonMaterials.Length; i++)
        {
            (lerpedColor, isMaterialRecolored) = lerpMaterialColors(seasonMaterials[i].getColor(), seasonMaterials[i].getSeasonColor());
            seasonMaterials[i].setColor(lerpedColor);
        }
    }

    #endregion

    #region Utilities

    /// <summary>
    /// If value is bigger then max, it is set to max
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    private float checkValueOverflow(float value, float maxValue)
    {
        if(value > maxValue) { value = maxValue; }

        return value;
    }

    private (Color, bool) lerpMaterialColors(Color previousColor, Color newColor)
    {
        Color lerpedColor = Color.Lerp(previousColor, newColor, Time.deltaTime * grassSwitchSpeed);

        float r = lerpedColor.r - newColor.r;
        r = r < 0 ? r * -1 : r;
        float g = lerpedColor.g - newColor.g;
        g = g < 0 ? g * -1 : g;
        float b = lerpedColor.b - newColor.b;
        b = b < 0 ? b * -1 : b;

        float colorSimilarity = r + g + b;

        return (lerpedColor, colorSimilarity < 0.1f);
    }

    #endregion

    [System.Serializable]
    public class GrassColor
    {
        public int terrainPosition;

        public Color springHealtyColor;
        public Color springDryColor;
        public Color summerHealtyColor;
        public Color summerDryColor;
        public Color autumnHealtyColor;
        public Color autumnDryColor;
        public Color winterHealtyColor;
        public Color winterDryColor;
    }

    [System.Serializable]
    public class WindSpeed
    {
        public float spring;
        public float summer;
        public float autumn;
        public float winter;
    }

    [System.Serializable]
    public class SeasonMaterial
    {
        public Material material;
        public string colorPropertyName;

        public Color springColor;
        public Color summerColor;
        public Color autumnColor;
        public Color winterColor;

        public Color getSeasonColor()
        {
            switch (season)
            {
                case seasons.spring:
                    return springColor;
                case seasons.summer:
                    return summerColor;
                case seasons.autumn:
                    return autumnColor;
                case seasons.winter:
                    return winterColor;
                default:
                    return new Color();
            }
        }

        public Color getPreviousSeasonColor()
        {
            switch (season)
            {
                case seasons.spring:
                    return winterColor;
                case seasons.summer:
                    return springColor;
                case seasons.autumn:
                    return summerColor;
                case seasons.winter:
                    return autumnColor;
                default:
                    return new Color();
            }
        }

        public void setColor(Color color)
        {
            material.SetColor(colorPropertyName, color);
        }

        public Color getColor()
        {
            return material.GetColor(colorPropertyName);
        }
    }

}
