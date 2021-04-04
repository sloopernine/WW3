using System;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public TMP_Text firepower;
    public TMP_Text angle;

    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject cannonControlsPanel;

    private bool updateText;

    private void OnDisable()
    {
        GameStateManager.INSTANCE.onChangeGameState -= OnGameStateChange;
    }

    private void Start()
    {
        GameStateManager.INSTANCE.onChangeGameState += OnGameStateChange;
        
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        cannonControlsPanel.SetActive(false);
    }

    private void Update()
    {
        if (updateText)
        {
            firepower.text = "FirePower: " + GameManager.INSTANCE.localPlayer.firePower.ToString("F2");
            angle.text = "Angle: " + GameManager.INSTANCE.localPlayer.cannon.transform.localEulerAngles.z.ToString("F2");
        }
        else
        {
            firepower.text = "";
            angle.text = "";
        }
    }

    private void OnGameStateChange(GameStateManager.GameState newGameState)
    {

        if (newGameState == GameStateManager.GameState.PlayersTurn)
        {
            Debug.Log("it is players turn");

            updateText = true;
            cannonControlsPanel.SetActive(true);            
        }
        else
        {
            Debug.Log("It is not players turn");

            updateText = false;
            cannonControlsPanel.SetActive(false);
        }
        
        if (newGameState == GameStateManager.GameState.Victory)
        {
            winPanel.SetActive(true);
        }

        if (newGameState == GameStateManager.GameState.GameOver)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void BackToMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ConfirmOutcomeAndLeaveGame()
    {
        StartCoroutine(FirebaseManager.INSTANCE.LoadData("games/" + DataManager.INSTANCE.GameData.gameID, GameManager.INSTANCE.LeaveGameAndEndTurn));
    }

    public void UpdatePowerAngleText()
    {
        firepower.text = "FirePower: " + GameManager.INSTANCE.localPlayer.firePower.ToString("F2");
        angle.text = "Angle: " + GameManager.INSTANCE.localPlayer.cannon.transform.localEulerAngles.z.ToString("F2");
    }

    public void DeactivateButtons()
    {
        cannonControlsPanel.SetActive(false);
    }
}
