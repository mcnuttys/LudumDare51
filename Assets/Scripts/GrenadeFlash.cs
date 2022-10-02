using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeFlash : MonoBehaviour
{
    [SerializeField] private AnimationCurve flashTimer;
    [SerializeField] private float maxTime = 10;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color flash;
    [SerializeField] private Color nonFlash;

    [SerializeField] private GameObject tickSound;

    private float timer;
    private float age = 0;
    private bool b = true;

    void Update()
    {
        age += Time.deltaTime;
        timer -= Time.deltaTime;

        if (timer <= 0)
            ResetTimer();

    }

    void ResetTimer()
    {
        timer = (flashTimer.Evaluate((maxTime - age) / maxTime)) * maxTime / 4;

        b = !b;
        sr.color = b ? flash : nonFlash;

        Instantiate(tickSound);
    }
}
