using System;
using System.Collections;
using Data.DataContainers;
using Data;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace Menu
{
    public class Lobby : MonoBehaviour
    {
        public GameObject gameSlot;
        public Transform contentParent;
        
        private void OnEnable()
        {
            StartCoroutine(InitialGameListUpdate());
        }

        public void JoinButton()
        {
            MainMenuManager.INSTANCE.ChangeState(MainMenuManager.MenuState.@join);
        }
        
        public void CreateGame()
        {
            if (ActiveUser.INSTANCE._userInfo.activeGames.Count >= 5)
            {
                MainMenuManager.INSTANCE.DisplayMessage("You already have to many games active", MainMenuManager.MenuState.lobby);
                return;
            }
            
            string date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string gameName = (ActiveUser.INSTANCE._userInfo.userID + date).GetHashCode().ToString("X");
            
            string key = FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Push().Key;

            PlayerInfo newPlayer = new PlayerInfo(ActiveUser.INSTANCE._userInfo.userID, ActiveUser.INSTANCE._userInfo.nickname);
         
            
            GameData gameData = new GameData();
            gameData.gameID = key;
            gameData.creationDate = date;
            gameData.gameName = gameName;
            gameData.creatorUserID = ActiveUser.INSTANCE._userInfo.userID;
            gameData.players.Add(newPlayer);
            gameData.currentTurn = 1; // 1 instead of 0 to better fit List.Count
            gameData.firstTurn = true;
            
            string path = "games/" + key;
            string data = JsonUtility.ToJson(gameData);
            
            FirebaseManager.INSTANCE.SaveData(path, data);

            ActiveGame newActiveGame = new ActiveGame();
            newActiveGame.gameID = gameData.gameID;
            
            ActiveUser.INSTANCE._userInfo.activeGames.Add(newActiveGame);
            ActiveUser.INSTANCE.SaveUserInfo();

            UpdateGameList();
        }
        
        private void UpdateGameList()
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var activeGame in ActiveUser.INSTANCE._userInfo.activeGames)
            {
                StartCoroutine(FirebaseManager.INSTANCE.LoadData("games/" + activeGame.gameID, AddGameToList));
            }
        }

        private void AddGameToList(string jsonData)
        {
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
            
            GameObject newGame = Instantiate(gameSlot, contentParent);
            GameSlot newGameSlot = newGame.GetComponent<GameSlot>();
            newGameSlot.gameName.text = gameData.gameName;
            newGameSlot.GameID = gameData.gameID;
        }

        IEnumerator InitialGameListUpdate()
        {
            // TODO Make this better by somehow checking that ActiveUser finished setting up
            yield return new WaitForSeconds(1);

            UpdateGameList();
        }
    }
}