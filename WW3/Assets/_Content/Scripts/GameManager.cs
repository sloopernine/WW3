using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayerInfo = Data.PlayerInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    public TMP_Text firepowerText;
    public TMP_Text angleText;

    public int maxActiveGames = 5;
    public int maxPlayers = 2;
    
    public Data.PlayerInfo playerInfo;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        playerInfo = new Data.PlayerInfo();
    }

    public void UpdateFirepower(float value)
    {
        playerInfo.firepower += value;
        //firepowerText.text = "Firepower: " + playerInfo.firepower.ToString("F2");
    }
    
    public void UpdateAngle(float value)
    {
        playerInfo.angle = value;
        //angleText.text = "Angle: " + playerInfo.angle.ToString("F2");
    }
}
