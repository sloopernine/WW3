using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public TMP_Text firepower;
    public TMP_Text angle;

    public GameObject winPanel;
    public GameObject GameOverPanel;
    
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
        GameOverPanel.SetActive(false);
    }

    private void OnGameStateChange(GameStateManager.GameState newGameState)
    {
        if (newGameState == GameStateManager.GameState.Victory)
        {
            winPanel.SetActive(true);
        }

        if (newGameState == GameStateManager.GameState.GameOver)
        {
            GameOverPanel.SetActive(true);
        }
    }

    public void BackToMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    private void Update()
    {
        //firepower.text = "Firepower: " + GameManager.INSTANCE.playerInfo.firepower.ToString();
        //angle.text = "Angle: " + GameManager.INSTANCE.playerInfo.angle.ToString();
    }
}
