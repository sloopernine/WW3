using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;
    
    public TMP_Text firepowerText;
    public TMP_Text angleText;

    private float firepower;
    private float angle;
    
    private void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(this);
    }

    public void UpdateFirepower(float value)
    {
        firepower += value;
        firepowerText.text = "Firepower: " + firepower.ToString("F2");
    }
    
    public void UpdateAngle(float value)
    {
        angle = value;
        angleText.text = "Angle: " + angle.ToString("F2");
    }
}
