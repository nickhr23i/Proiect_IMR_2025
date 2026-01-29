using UnityEngine;

public partial class StylizedDayNight : MonoBehaviour
{
    [Header("Cycle Settings")]
    [Tooltip("How long a full day/night cycle lasts in seconds.")]
    public float cycleDuration = 60f;

    [Header("Color Palette")]
    [Tooltip("Set the colors here. The script will blend between these over time.")]
    public Gradient skyColors;
    public Gradient fogColors;

    [Header("Skybox Material")]
    [Tooltip("Use a 'Skybox/Procedural' or 'Skybox/6 Sided' material.")]
    public Material skyboxMaterial;

    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime / cycleDuration;
        if (_timer > 1f) _timer = 0f;

        Color currentSkyColor = skyColors.Evaluate(_timer);
        Color currentFogColor = fogColors.Evaluate(_timer);

        if (skyboxMaterial != null)
        {
            // This checks for the two most common Skybox color variable names
            if (skyboxMaterial.HasProperty("_Tint"))
                skyboxMaterial.SetColor("_Tint", currentSkyColor);
            else if (skyboxMaterial.HasProperty("_SkyTint"))
                skyboxMaterial.SetColor("_SkyTint", currentSkyColor);
        }

        RenderSettings.fogColor = currentFogColor;
        RenderSettings.ambientLight = currentSkyColor;
    }
}