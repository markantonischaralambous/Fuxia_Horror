using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    public Image blinkScreen;

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(7f); // wait before blinking

        for (int i = 0; i < 5; i++)
        {
            SetBlack(true);   // BLACK
            yield return new WaitForSeconds(0.4f);

            SetBlack(false);  // VISIBLE
            yield return new WaitForSeconds(0.4f);
        }

        SetBlack(true); // final blackout (faint)
    }

    void SetBlack(bool state)
    {
        Color c = blinkScreen.color;
        c.a = state ? 1f : 0f;
        blinkScreen.color = c;
    }
}