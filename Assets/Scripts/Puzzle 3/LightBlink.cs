using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    public Light2D myLight;
    public string blinkSequence = " . - - .- - - . - . . . - ";

    public float dotTime = 0.3f;
    public float dashTime = 1f;
    public float pauseTime = 0.4f;

    public float pauseBeforeRepeating;
    void Start()
    {
        StartCoroutine(PlayMorse());
    }

    IEnumerator PlayMorse()
    {
        while (true)
        {
            foreach (char c in blinkSequence)
            {
                if (c == '.')
                {
                    yield return StartCoroutine(Blink(dotTime));
                }
                else if (c == '-')
                {
                    yield return StartCoroutine(Blink(dashTime));
                }
                else if (c == ' ')
                {
                    yield return new WaitForSeconds(1f);
                }
            }

            yield return new WaitForSeconds(pauseBeforeRepeating); // pause before repeating
        }
    }

    IEnumerator Blink(float duration)
    {
        myLight.enabled = true;
        yield return new WaitForSeconds(duration);

        myLight.enabled = false;
        yield return new WaitForSeconds(pauseTime);
    }
}
