using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;


    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    [SerializeField] private float paddingX = 1f;
    [SerializeField] private float paddingY = 1f;

    void Start ()
	{
	    SetUpMoveBorders();
	}

    private void SetUpMoveBorders()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingX;
        xMax = gameCamera.ViewportToScreenPoint(new Vector3(0.0075f, 0, 0)).x - paddingX;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingY;
        yMax = gameCamera.ViewportToScreenPoint(new Vector3(0, 0.0055f, 0)).y + paddingY;
    }

    void Update ()
	{
	    Move();
	}

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax); 
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
}
