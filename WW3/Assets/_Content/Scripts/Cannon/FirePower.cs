using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePower : MonoBehaviour
{
    public float firepowerStep;

    public CannonController cannonController;

    private bool isPressed = false;
    
    void Start()
    {
        cannonController = GetComponentInParent<CannonController>();
    }

    private void OnMouseOver()
    {
        if(isPressed)
            cannonController.SetFirepower(firepowerStep * Time.deltaTime);
    }
    
    private void OnMouseDown()
    {
        isPressed = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        //play some lock sound
    }
}
