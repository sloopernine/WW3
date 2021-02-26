using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCannon : MonoBehaviour
{
    public float rotationSpeed;

    public CannonController cannonController;

    private bool isPressed = false;
    
    void Start()
    {
        cannonController = GetComponentInParent<CannonController>();
    }

    private void OnMouseOver()
    {
        if(isPressed)
            cannonController.RotateCannon(rotationSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        isPressed = true;
        cannonController.StartRotateCannon();
    }

    private void OnMouseExit()
    {
        isPressed = false;
        cannonController.StopRotateCannon();
    }

    private void OnMouseUp()
    {
        isPressed = false;
        cannonController.StopRotateCannon();
    }
}
