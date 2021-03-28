using Data;
using Data.DataContainers;
using Firebase.Database;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSlot : MonoBehaviour
{
    private FirebaseDatabase _database;
    private ActiveUser _activeUser;
    private DataManager _dataManager;
    
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
        
        _activeUser = ActiveUser.INSTANCE;
        _dataManager = DataManager.INSTANCE;
    }

    public void StartGame()
    {
        StartCoroutine(FirebaseManager.INSTANCE.LoadData("games/" + gameID, LoadScene));
    }

    public void LoadScene(string jsonData)
    {
        DataManager.INSTANCE.GameData = JsonUtility.FromJson<GameData>(jsonData);

        int index = _activeUser.GetIndexByGameID(_dataManager.GameData.gameID);
        
        //TODO Move this into GameLoop scene and set initial firepower and angle based on start location
        if (_activeUser._userInfo.activeGames[index].firstStart)
        {
            PopulatePlayerDataInActiveGame(index);
        }
        
        SceneManager.LoadScene("_Content/Scenes/GameScene");
    }

    private void PopulatePlayerDataInActiveGame(int index)
    {
        foreach (var player in _dataManager.GameData.players)
        {
            PlayerList newPlayer = new PlayerList();
            newPlayer.isAlive = true;
            newPlayer.userID = player.playerID;
                
            _activeUser._userInfo.activeGames[index].playerList.Add(newPlayer);
        }

        _activeUser._userInfo.activeGames[index].firstStart = false;
            
        _activeUser.SaveUserInfo();
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
