using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 3;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, followSpeed * Time.fixedDeltaTime);
    }

    public void SetTarget(Transform t)
    {
        transform.position = t.position;
        target = t;
    }
}
