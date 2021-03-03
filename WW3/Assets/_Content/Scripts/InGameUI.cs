using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public TMP_Text firepower;
    public TMP_Text angle;

    private void Update()
    {
        //firepower.text = "Firepower: " + GameManager.INSTANCE.playerInfo.firepower.ToString();
        //angle.text = "Angle: " + GameManager.INSTANCE.playerInfo.angle.ToString();
    }
}
