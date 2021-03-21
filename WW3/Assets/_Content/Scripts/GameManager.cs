using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Firebase.Auth;
using Firebase.Database;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerInfo = Data.DataContainers.PlayerInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    private DataManager _dataManager;

    public GameObject opponentsTurnText;

    public List<CannonController> _players;

    public CannonController localPlayer;
    
    #region Settings

    public int maxActiveGames = 5;
    public int maxPlayers = 2;

    #endregion
    
    public delegate void OnReplayFinished();
    
    private void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(gameObject);
        
        _dataManager = DataManager.INSTANCE;
        _players = new List<CannonController>();
    }

    private void Start()
    {
        GameStateManager.INSTANCE.onChangeGameState += OnGameStateChanged;
        FirebaseDatabase.DefaultInstance.GetReference("games/" + _dataManager.GameData.gameID).Child("currentTurn").ValueChanged += TurnUpdated;
        CollectPlayers();
        localPlayer = _players[GetLocalPlayerIndex()];
    }

    public void CollectPlayers()
    {
        int index = 0;
        
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            CannonController cannonController = player.GetComponent<CannonController>();
            cannonController.playerIndex = index; 
            
            _players.Add(cannonController);
            index++;
        }
    }

    private void OnDisable()
    {
        GameStateManager.INSTANCE.onChangeGameState -= OnGameStateChanged;
        FirebaseDatabase.DefaultInstance.GetReference("games/" + _dataManager.GameData.gameID).Child("currentTurn").ValueChanged -= TurnUpdated;
    }
    
    void TurnUpdated(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        
        StartCoroutine(FirebaseManager.INSTANCE.LoadData("games/" + _dataManager.GameData.gameID, GameDataUpdated));
    }

    void GameDataUpdated(string jsonData)
    {
        _dataManager.GameData = JsonUtility.FromJson<Data.DataContainers.GameData>(jsonData);
        
        if (GetIfLocalPlayerTurn())
        {
            if (_dataManager.GameData.firstTurn)
            {
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.PlayersTurn);
            }
            else
            {
                StartCoroutine(ShowReplay(ReplayFinished));
            }
        }
        else
        {
            GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.OpponentTurn);
        }
    }

    IEnumerator ShowReplay(OnReplayFinished onReplayFinished)
    {
        int index = GetLastTurn(_dataManager.GameData.currentTurn);
        CannonController player = _players[index];
        
        player.firePower = _dataManager.GameData.players[index].firepower;
        player.SetAngle(_dataManager.GameData.players[index].angle);
        
        yield return new WaitForSeconds(1.5f);
        player.FireCannon();

        yield return new WaitForSeconds(5);
        
        onReplayFinished();
    }

    void ReplayFinished()
    {
        int playersAlive = 0;

        foreach (var player in _dataManager.GameData.players)
        {
            if (player.isAlive)
                playersAlive += 1;
        }

        if (playersAlive >= 2)
        {
            if (_dataManager.GameData.players[_dataManager.GameData.currentTurn].isAlive)
            {
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.PlayersTurn);
            }
            else
            {
                Debug.Log("You are dead, war goes on without you");
                ActiveUser.INSTANCE.RemoveActiveGame(_dataManager.GameData.gameID);
                _dataManager.RemovePlayerFromGame(_dataManager.GameData.gameID);
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.GameOver);
            }
        }
        else
        {
            if (_dataManager.GameData.players[_dataManager.GameData.currentTurn].isAlive)
            {
                Debug.Log("You won, gz!");
                ActiveUser.INSTANCE.RemoveActiveGame(_dataManager.GameData.gameID);
                _dataManager.RemovePlayerFromGame(_dataManager.GameData.gameID);
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.Victory);
            }
            else
            {
                Debug.Log("You are dead, war goes on without you");
                ActiveUser.INSTANCE.RemoveActiveGame(_dataManager.GameData.gameID);
                _dataManager.RemovePlayerFromGame(_dataManager.GameData.gameID);
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.GameOver);
            }
        }
    }
    
    void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        if (gameState == GameStateManager.GameState.PlayersTurn)
        {
            opponentsTurnText.SetActive(false);
        }
        else if(gameState == GameStateManager.GameState.OpponentTurn)
        {
            opponentsTurnText.SetActive(true);
        }
    }

    public bool GetIfLocalPlayerTurn()
    {
        if (_dataManager.GameData.players[_dataManager.GameData.currentTurn].playerID == ActiveUser.INSTANCE._userInfo.userID)
        {
            return true;
        }

        return false;
    }

    private int GetLocalPlayerIndex()
    {
        int index = 0;
        
        foreach (PlayerInfo player in _dataManager.GameData.players)
        {
            if (player.playerID == ActiveUser.INSTANCE._userInfo.userID)
            {
                return index;
            }

            index++;
        }

        return -1;
    }
    
    private int GetLastTurn(int currentTurn)
    {
        // TODO Need more work if more than two players
        if (currentTurn == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    
    private int GetNextTurn(int currentTurn)
    {
        // TODO Need more work if more than two players
        if (currentTurn == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void EndTurn(float firepower, float angle)
    {
        _dataManager.GameData.players[_dataManager.GameData.currentTurn].firepower = firepower;
        _dataManager.GameData.players[_dataManager.GameData.currentTurn].angle = angle;
        _dataManager.GameData.currentTurn = GetNextTurn(_dataManager.GameData.currentTurn);

        if (_dataManager.GameData.firstTurn)
        {
            _dataManager.GameData.firstTurn = false;
        }

        string jsonData = JsonUtility.ToJson(_dataManager.GameData);
        
        FirebaseManager.INSTANCE.SaveData("games/" + _dataManager.GameData.gameID, jsonData);
    }
}
