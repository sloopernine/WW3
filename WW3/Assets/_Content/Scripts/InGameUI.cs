using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public TMP_Text firepower;
    public TMP_Text angle;

    public GameObject winPanel;
    public GameObject losePanel;
    
    private void OnEnable()
    {
        GameStateManager.INSTANCE.onChangeGameState += OnGameStateChange;
    }

    private void OnDisable()
    {
        GameStateManager.INSTANCE.onChangeGameState -= OnGameStateChange;
    }

    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void OnGameStateChange(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.Victory)
        {
            winPanel.SetActive(true);
        }

        if (newGameState == GameStateManager.GameState.GameOver)
        {
            losePanel.SetActive(true);
        }
    }
    
    private void Update()
    {
        //firepower.text = "Firepower: " + GameManager.INSTANCE.playerInfo.firepower.ToString();
        //angle.text = "Angle: " + GameManager.INSTANCE.playerInfo.angle.ToString();
    }
}
