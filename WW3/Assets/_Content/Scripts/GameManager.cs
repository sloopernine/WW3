using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using PlayerInfo = Data.PlayerInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;
    
    public int maxActiveGames = 5;
    public int maxPlayers = 2;
    
    private GameData _gameData;

    private string activeGameID;

    public string ActiveGameID
    {
        get => activeGameID;
        set
        {
            //TODO Somehow load GameScene here or make SceneManager and call from here
            activeGameID = value;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //playerInfo = new Data.PlayerInfo();
    }

    public void UpdateFirepower(float value)
    {
        //playerInfo.firepower += value;
    }
    
    public void UpdateAngle(float value)
    {
        //playerInfo.angle = value;
    }
}
