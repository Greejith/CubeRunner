using UnityEngine;

public class PulseGlow : MonoBehaviour
{
    public Material glowMaterial;
    public Color glowColor = Color.cyan;
    public float baseIntensity = 1f;
    public float blinkSpeed = 2f;
    public float blinkStrength = 0.5f;

    void Update()
    {
        float intensity = baseIntensity + Mathf.Sin(Time.time * blinkSpeed) * blinkStrength;
        Color finalColor = glowColor * Mathf.LinearToGammaSpace(intensity);
        glowMaterial.SetColor("_EmissionColor", finalColor);
    }
}
