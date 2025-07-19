using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JellyWaveController : MonoBehaviour
{
    public float pulseScale = 1.1f;
    public float pulseDuration = 0.3f;
    public float delayBetween = 0.15f;

    private RectTransform[] buttons;
    private Vector3[] originalScales;

    void Start()
    {
        Button[] btns = GetComponentsInChildren<Button>();
        buttons = new RectTransform[btns.Length];
        originalScales = new Vector3[btns.Length];

        for (int i = 0; i < btns.Length; i++)
        {
            buttons[i] = btns[i].GetComponent<RectTransform>();
            originalScales[i] = buttons[i].localScale;
        }

        StartCoroutine(PlayJellyWave());
    }

    IEnumerator PlayJellyWave()
    {
        while (true)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                RectTransform btn = buttons[i];
                Vector3 original = originalScales[i];

                LeanTween.scale(btn, original * pulseScale, pulseDuration / 2).setEaseOutQuad();
                yield return new WaitForSeconds(pulseDuration / 2);

                LeanTween.scale(btn, original, pulseDuration / 2).setEaseOutBounce();
                yield return new WaitForSeconds(delayBetween);
            }
        }
    }
}
