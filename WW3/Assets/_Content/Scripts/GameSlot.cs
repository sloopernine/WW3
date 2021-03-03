using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSlot : MonoBehaviour
{
    private FirebaseDatabase _database;
    
    public TMP_Text gameName;
    public Button startButton;
    public TMP_Text playerName1;
    public TMP_Text playerName2;

    private string gameID;
    
    public string GameID
    {
        get => gameID;
        set
        {
            gameID = value;

            _database.GetReference("games/" + gameID).ValueChanged += PlayersUpdated;
        }
    }

    private void OnDisable()
    {
        _database.GetReference("games/" + gameID).ValueChanged -= PlayersUpdated;
    }

    private void Awake()
    {
        _database = FirebaseDatabase.DefaultInstance;
    }

    private void Start()
    {
        startButton.interactable = false;
    }

    public void CopyGameName()
    {
        UniClipboard.SetText(gameName.text);
    }

    private void PlayersUpdated(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        string jsonData = args.Snapshot.GetRawJsonValue();

        GameData gameData = new GameData();
        
        gameData = JsonUtility.FromJson<GameData>(jsonData);

        playerName1.text = gameData.players[0].nickname;
        
        if (gameData.players.Count >= 2)
        {
            playerName2.text = gameData.players[1].nickname;
            startButton.interactable = true;
        }
    }
}
