using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Firebase.Database;
using FirebaseProject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using PlayerInfo = Data.PlayerInfo;

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
            if (ActiveUser.INSTANCE._userInfo.activeGames.Count >= GameManager.INSTANCE.maxActiveGames)
            {
                MainMenuManager.INSTANCE.DisplayMessage("You already have to many games active", MainMenuManager.MenuState.lobby);
                return;
            }
            
            string date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string gameName = (ActiveUser.INSTANCE._userInfo.userID + date).GetHashCode().ToString("X");
            
            string key = FirebaseDatabase.DefaultInstance.RootReference.Child("games/").Push().Key;

            Data.PlayerInfo newPlayer = new Data.PlayerInfo();
            newPlayer = NewPlayer(ActiveUser.INSTANCE._userInfo.nickname, ActiveUser.INSTANCE._userInfo.userID);
            
            GameData gameData = new GameData();
            gameData.gameID = key;
            gameData.creationDate = date;
            gameData.gameName = gameName;
            gameData.players.Add(newPlayer);

            string path = "games/" + key;
            string data = JsonUtility.ToJson(gameData);
            
            Data.FirebaseManager.INSTANCE.SaveData(path, data);
            
            ActiveUser.INSTANCE._userInfo.activeGames.Add(gameData.gameName);
            ActiveUser.INSTANCE.SaveUserInfo();

            UpdateGameList();
        }

        private Data.PlayerInfo NewPlayer(string nickname, string playerID)
        {
            Data.PlayerInfo playerInfo = new Data.PlayerInfo();

            playerInfo.playerID = playerID;
            playerInfo.nickname = nickname;
            playerInfo.position = Vector2.zero;
            playerInfo.isAlive = true;
            playerInfo.money = 500;
            playerInfo.angle = 0;
            playerInfo.firepower = 0;

            return playerInfo;
        }

        private void UpdateGameList()
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var gameName in ActiveUser.INSTANCE._userInfo.activeGames)
            {
                GameObject newGame = Instantiate(gameSlot, contentParent);
                newGame.GetComponent<GameSlot>().gameName.text = gameName;
            }
        }

        IEnumerator InitialGameListUpdate()
        {
            // TODO Make this better by somehow checking that ActiveUser finished setting up
            yield return new WaitForSeconds(1);

            UpdateGameList();
        }
    }
}