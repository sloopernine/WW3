using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Firebase.Database;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void StartGame()
    {
        StartCoroutine(FirebaseManager.INSTANCE.LoadData("games/" + gameID, LoadScene));
    }

    public void LoadScene(string jsonData)
    {
        DataManager.INSTANCE.GameData = JsonUtility.FromJson<Data.DataContainers.GameData>(jsonData);
        SceneManager.LoadScene("_Content/Scenes/GameScene");
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

        Data.DataContainers.GameData gameData = new Data.DataContainers.GameData();
        
        gameData = JsonUtility.FromJson<Data.DataContainers.GameData>(jsonData);

        playerName1.text = gameData.players[0].nickname;
        
        if (gameData.players.Count >= 2)
        {
            playerName2.text = gameData.players[1].nickname;
            startButton.interactable = true;
        }
    }
}
