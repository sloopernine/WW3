using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{
    public float firePower;

    public CannonController cannonController;

    private bool isPressed = false;
    
    void Start()
    {
        cannonController = GetComponentInParent<CannonController>();
    }

    private void OnMouseOver()
    {
        // if(isPressed)
        //     cannonController.FireCannon();
    }

    private void OnMouseDown()
    {
        cannonController.FireCannon();
        cannonController.EndTurn();
    }

    private void OnMouseUp()
    {
        isPressed = false;
        //play some lock sound
    }
}
