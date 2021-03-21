using System;
using System.Collections;
using Managers;
using UnityEngine;

public class InGameInputUI : MonoBehaviour
{
    public float firepowerStep;
    public float rotationStep;

    private bool beginToRotate;

    private void Start()
    {
        beginToRotate = true;
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
            GameManager.INSTANCE.localPlayer.StartRotateCannon();
            beginToRotate = false;
        }

        StartCoroutine(RotateAntiClockwizeLoop());
    }
    
    private IEnumerator RotateClockwizeLoop()
    {
        while(true)
        {
            GameManager.INSTANCE.localPlayer.RotateCannon((rotationStep * -1) * Time.deltaTime);
            yield return null;
        }
    }
    
    IEnumerator RotateAntiClockwizeLoop()
    {
        while (true)
        {
            GameManager.INSTANCE.localPlayer.RotateCannon(rotationStep * Time.deltaTime);
            yield return null;
        }
    }

    public void StopRotating()
    {
        GameManager.INSTANCE.localPlayer.StopRotateCannon();
        beginToRotate = true;
        
        StopAllCoroutines();
    }

    public void DecreasePower()
    {
        GameManager.INSTANCE.localPlayer.SetFirepower((firepowerStep * -1) * Time.deltaTime);
    }

    public void IncreasePower()
    {
        GameManager.INSTANCE.localPlayer.SetFirepower(firepowerStep * Time.deltaTime);
    }

    public void Fire()
    {
        GameManager.INSTANCE.localPlayer.FireCannon();
        GameManager.INSTANCE.localPlayer.EndTurn();
    }
}
