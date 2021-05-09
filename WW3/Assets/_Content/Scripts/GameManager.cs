using System;
using System.Collections;
using System.Collections.Generic;
using _Content.Scripts;
using Data;
using Data.Containers;
using Data.DataContainers;
using Firebase.Auth;
using Firebase.Database;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    private DataManager _dataManager;

    public GameObject statusTextRoot;
    public TMP_Text statusText;

    public List<CannonController> players;
    public CannonController localPlayer;

    private AudioSource audioSource;
    public AudioClip yourTurnDrum;
    
    public delegate void OnReplayFinished();
    
    private void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(gameObject);
        
        _dataManager = DataManager.INSTANCE;
        players = new List<CannonController>();
    }

    private void Start()
    {
        GameStateManager.INSTANCE.onChangeGameState += OnGameStateChanged;
        FirebaseDatabase.DefaultInstance.GetReference("games/" + _dataManager.GameData.gameID).Child("currentTurn").ValueChanged += TurnUpdated;
        CollectPlayers();

        SetupLocalPlayer(GetLocalPlayerIndex());
        
        audioSource = GetComponent<AudioSource>();
    }

    public void CollectPlayers()
    {
        int index = 0;
        
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            CannonController cannonController = player.GetComponent<CannonController>();
            cannonController.playerIndex = index; 
            
            players.Add(cannonController);
            index++;
        }
    }

    private void SetupLocalPlayer(int index)
    {
        localPlayer = players[index];
        localPlayer.SetInitialValues(_dataManager.GameData.players[index].firepower, _dataManager.GameData.players[index].angle);
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
        _dataManager.GameData = JsonUtility.FromJson<GameData>(jsonData);
        
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
        statusText.text = "REPLAY";
        statusText.enabled = true;
        
        int index = GetLastPlayersTurn(_dataManager.GameData.currentPlayerTurn);
        CannonController player = players[index];
        
        player.firePower = _dataManager.GameData.players[index].firepower;
        player.SetAngle(_dataManager.GameData.players[index].angle);
        
        yield return new WaitForSeconds(1.5f);
        GameObject cannonball = player.FireCannon();

        while (cannonball != null)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1);
        
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
            if (_dataManager.GameData.currentPlayerTurn == localPlayer.playerIndex)
            {
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.PlayersTurn);
            }
            else
            {
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.OpponentTurn);
            }
        }
        else
        {
            if (_dataManager.GameData.players[_dataManager.GameData.currentPlayerTurn].isAlive)
            {
                statusTextRoot.SetActive(false);
                statusText.enabled = false;
                Debug.Log("You won, gz!");
                //ActiveUser.INSTANCE.RemoveActiveGame(_dataManager.GameData.gameID);
                _dataManager.RemovePlayerFromGame(_dataManager.GameData.gameID);
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.Victory);
            }
            else
            {
                statusTextRoot.SetActive(false);
                statusText.enabled = false;
                Debug.Log("You are dead, war goes on without you");
                //ActiveUser.INSTANCE.RemoveActiveGame(_dataManager.GameData.gameID);
                _dataManager.RemovePlayerFromGame(_dataManager.GameData.gameID);
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.GameOver);
            }
        }
    }
    
    void OnGameStateChanged(GameStateManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameStateManager.GameState.PlayersTurn:

                statusText.enabled = false;
                //localPlayer.SendSignal(Signal.StartTurn);
                audioSource.PlayOneShot(yourTurnDrum);
                
                break;
            
            case GameStateManager.GameState.OpponentTurn:
                
                statusText.text = "OPPONENTS TURN";
                statusText.enabled = true;
                
                break;
            
            case GameStateManager.GameState.Victory:
                
                statusTextRoot.SetActive(false);
                statusText.enabled = false;
                
                break;
        }
    }

    public bool GetIfLocalPlayerTurn()
    {
        if (_dataManager.GameData.players[_dataManager.GameData.currentPlayerTurn].playerID == ActiveUser.INSTANCE._userInfo.userID)
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
    
    private int GetLastPlayersTurn(int currentPlayersTurn)
    {
        // TODO Need more work if more than two players
        if (currentPlayersTurn == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    
    private int GetNextPlayersTurn(int currentPlayersTurn)
    {
        // TODO Need more work if more than two players
        if (currentPlayersTurn == 0)
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
        _dataManager.GameData.players[_dataManager.GameData.currentPlayerTurn].firepower = firepower;
        _dataManager.GameData.players[_dataManager.GameData.currentPlayerTurn].angle = angle;
        _dataManager.GameData.currentTurn += 1;
        _dataManager.GameData.currentPlayerTurn = GetNextPlayersTurn(_dataManager.GameData.currentPlayerTurn);

        if (_dataManager.GameData.firstTurn)
        {
            _dataManager.GameData.firstTurn = false;
        }

        int index = ActiveUser.INSTANCE.GetIndexByGameID(_dataManager.GameData.gameID);
        int playersAlive = 0;
        
        foreach (var player in ActiveUser.INSTANCE._userInfo.activeGames[index].playerList)
        {
            if (player.isAlive)
                playersAlive += 1;
        }

        if(playersAlive == 1)
        {
            if (ActiveUser.INSTANCE._userInfo.activeGames[index].playerList[localPlayer.playerIndex].isAlive)
            {
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.Victory);
            }
            else
            {
                GameStateManager.INSTANCE.ChangeGameState(GameStateManager.GameState.GameOver);
            }
        }

        string jsonData = JsonUtility.ToJson(_dataManager.GameData);
        
        FirebaseManager.INSTANCE.SaveData("games/" + _dataManager.GameData.gameID, jsonData);
    }

    public void LeaveGameAndEndTurn(string data)
    {
        ActiveUser.INSTANCE.RemoveActiveGame(_dataManager.GameData.gameID);
        
        // int counter = 0;
        //
        // foreach (var player in _dataManager.GameData.players)
        // {
        //     if (player.leftGame)
        //     {
        //         counter++;
        //     }
        // }
        //
        // if (counter >= Settings.MAX_PLAYERS_GAME - 1)
        // {
        //     StartCoroutine(FirebaseManager.INSTANCE.RemoveGame(_dataManager.GameData.gameID));
        // }
        // else
        // {
        //     _dataManager.GameData.currentTurn += 1;
        //     _dataManager.GameData.currentPlayerTurn = GetNextPlayersTurn(_dataManager.GameData.currentPlayerTurn);
        //     _dataManager.GameData.players[localPlayer.playerIndex].leftGame = true;
        //     
        //     string jsonData = JsonUtility.ToJson(_dataManager.GameData);
        //     
        //     FirebaseManager.INSTANCE.SaveData("games/" + _dataManager.GameData.gameID, jsonData);
        // }
        
        SceneManager.LoadScene("MainMenu");
    }
}
