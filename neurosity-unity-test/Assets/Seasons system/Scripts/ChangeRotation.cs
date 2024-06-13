using UnityEngine;

public class ChangeRotation : MonoBehaviour
{
    public float startRotation;
    public float endRotation;
    public float speed;
    public float fogColorSwitchSpeed;

    public Color dayFogColor;
    public Color nightFogColor;

    public Light directionalLight;

    void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.T)) { Debug.Log(transform.localEulerAngles.x); }

        if(transform.localEulerAngles.x > 240 && transform.localEulerAngles.x < 360)
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, nightFogColor, Time.deltaTime * fogColorSwitchSpeed);
            if (directionalLight.intensity > 0) { directionalLight.intensity -= Time.deltaTime * 2; }
            if (RenderSettings.ambientIntensity > 0.15) { RenderSettings.ambientIntensity -= Time.deltaTime * 2; }
        }
        else
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, dayFogColor, Time.deltaTime * fogColorSwitchSpeed);
            if (directionalLight.intensity < 1) { directionalLight.intensity += Time.deltaTime * 2; }
            if (RenderSettings.ambientIntensity < 0.8) { RenderSettings.ambientIntensity += Time.deltaTime * 2; }
        }


    }
}
