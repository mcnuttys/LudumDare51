using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    private float maxAge = 5;

    private void Update()
    {
        maxAge -= Time.deltaTime;

        if(maxAge <= 0)
            Destroy(gameObject);
    }
}
