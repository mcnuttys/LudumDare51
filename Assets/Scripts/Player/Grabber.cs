using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private float maxDistanceFromPlayer = 1;
    [SerializeField] private float kickForce = 1000;
    [SerializeField] private float maxDragForce = 1000;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color grabColor;
    [SerializeField] private Color idleColor;

    [SerializeField] SpriteRenderer grabber;

    private GameObject onBox;
    private Vector2 grabPos;

    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        if (onBox == null) return;
        var pos = (Vector2)transform.position;
        var boxPos = (Vector2)onBox.transform.position;

        if (Input.GetKeyDown(KeyCode.Space))
            onBox.GetComponent<Rigidbody2D>().AddForce((pos - (Vector2)transform.parent.position).normalized * kickForce);

        if (Input.GetMouseButtonDown(0))
        {
            grabPos = pos - boxPos;
            grabber.color = grabColor;
        }

        if (Input.GetMouseButton(0))
        {
            onBox.gameObject.layer = 9;
            if (grabPos == Vector2.zero) return;

            var gGrabPos = boxPos + grabPos;
            var dragForce = (pos - gGrabPos) * maxDragForce * Time.deltaTime;

            onBox.GetComponent<Rigidbody2D>().AddForceAtPosition(dragForce, boxPos + grabPos);
        }

        if (Input.GetMouseButtonUp(0) || Vector2.Distance(boxPos + grabPos, mousePos) >= maxDistanceFromPlayer)
        {
            onBox.gameObject.layer = 8;
            onBox = null;
            grabPos = Vector2.zero;

            grabber.color = idleColor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (onBox != null) return;

        if (collision.gameObject.tag == "Box")
        {
            grabber.color = hoverColor;
            onBox = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (onBox != null) return;

        grabber.color = idleColor;
    }
}
