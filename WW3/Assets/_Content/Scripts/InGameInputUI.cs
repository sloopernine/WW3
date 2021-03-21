using System;
using System.Collections;
using Managers;
using UnityEngine;

public class InGameInputUI : MonoBehaviour
{
    private CannonController localPlayer;
    
    public float firepowerStep;
    public float rotationStep;

    private bool beginToRotate;
    private bool beginToSetPower;
    
    private void Start()
    {
        localPlayer = GameManager.INSTANCE.localPlayer;
        
        beginToRotate = true;
        beginToSetPower = true;
    }
    
    public void RotateClockwise()
    {
        if (beginToRotate)
        {
            GameManager.INSTANCE.localPlayer.StartRotateCannon();
            beginToRotate = false;
        }

        StartCoroutine(RotateClockwizeLoop());
    }
    
    public void RotateAntiClockwise()
    {
        if (beginToRotate)
        {
            localPlayer.StartRotateCannon();
            beginToRotate = false;
        }

        StartCoroutine(RotateAntiClockwizeLoop());
    }
    
    private IEnumerator RotateClockwizeLoop()
    {
        while(true)
        {
            localPlayer.RotateCannon((rotationStep * -1) * Time.deltaTime);
            yield return null;
        }
    }
    
    IEnumerator RotateAntiClockwizeLoop()
    {
        while (true)
        {
            localPlayer.RotateCannon(rotationStep * Time.deltaTime);
            yield return null;
        }
    }

    public void StopRotating()
    {
        localPlayer.StopRotateCannon();
        beginToRotate = true;
        
        StopAllCoroutines();
    }

    public void DecreasePower()
    {
        if (beginToSetPower)
        {
            localPlayer.StartToSetPower();
            beginToSetPower = false;
        }
        
        localPlayer.SetFirepower((firepowerStep * -1) * Time.deltaTime);
    }

    public void IncreasePower()
    {
        if (beginToSetPower)
        {
            localPlayer.StartToSetPower();
            beginToSetPower = false;
        }
        
        localPlayer.SetFirepower(firepowerStep * Time.deltaTime);
    }

    public void StopSetPower()
    {
        localPlayer.StopSetFirepower();
        beginToSetPower = true;
    }

    public void Fire()
    {
        localPlayer.FireCannon();
        localPlayer.EndTurn();
    }
}
