using System.Collections;
using System.Collections.Generic;
using Data;
using Firebase.Database;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class JoinPanel : MonoBehaviour
    {
        public TMP_InputField inviteCodeInput;

        public void AddGameToList()
        {
            string inviteCode = inviteCodeInput.text;

            //Check if empty
            if (inviteCode == "")
            {
                MainMenuManager.INSTANCE.DisplayMessage("Invite Code cant be empty", MainMenuManager.MenuState.@join);
            }
            
            //Check if already joined to many games
            if (ActiveUser.INSTANCE._userInfo.activeGames.Count >= GameManager.INSTANCE.maxActiveGames)
            {
                inviteCodeInput.text = "";
                MainMenuManager.INSTANCE.DisplayMessage("You already have to many games active", MainMenuManager.MenuState.lobby);
            }

            //Check if already joined this game
            foreach (var gameName in ActiveUser.INSTANCE._userInfo.activeGames)
            {
                if (gameName == inviteCode)
                {
                    inviteCodeInput.text = "";
                    MainMenuManager.INSTANCE.DisplayMessage("You have already joined this game", MainMenuManager.MenuState.lobby);
                }
            }

            StartCoroutine(CheckForGame(inviteCode));
            
            //TODO If everything checks out, add the game to player active games
        }

        IEnumerator CheckForGame(string inviteCode)
        {
            var dataTask = FirebaseDatabase.DefaultInstance.GetReference("games").OrderByChild("gameName").EqualTo(inviteCode).GetValueAsync();

            yield return new WaitUntil(() => dataTask.IsCompleted);
            
            //string data = dataTask.Result.GetRawJsonValue();
            string data = "";
            
            if (dataTask != null)
                Debug.Log(dataTask.Exception);

            //Check if game exist @ firebase
            if (dataTask.Result.ChildrenCount == 0)
            {
                MainMenuManager.INSTANCE.DisplayMessage("No game found with the invite code " + inviteCode, MainMenuManager.MenuState.@join);
                yield break;
            }
            else
            {
                foreach (var child in dataTask.Result.Children)
                {
                    data = child.GetRawJsonValue();
                }

                GameData gameData = new GameData();
                gameData = JsonUtility.FromJson<GameData>(data);
                
                //Check if the selected game have open player slots
                if (gameData.players.Count >= GameManager.INSTANCE.maxPlayers)
                {
                    MainMenuManager.INSTANCE.DisplayMessage("No free player slots in game", MainMenuManager.MenuState.lobby);
                    yield break;
                }
                else
                {
                    string path = "games/" + gameData.gameID;
                    
                    Data.PlayerInfo newPlayer = new Data.PlayerInfo();
                    newPlayer = NewPlayer(ActiveUser.INSTANCE._userInfo.nickname, ActiveUser.INSTANCE._userInfo.userID);
                    
                    gameData.players.Add(newPlayer);

                    string newData = JsonUtility.ToJson(gameData);
                    
                    Data.FirebaseManager.INSTANCE.SaveData(path, newData);
                    
                    ActiveUser.INSTANCE._userInfo.activeGames.Add(gameData.gameName);
                    ActiveUser.INSTANCE.SaveUserInfo();
                    
                    MainMenuManager.INSTANCE.DisplayMessage("Game " + inviteCode + " added", MainMenuManager.MenuState.lobby);
                }
            }
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

        public void PasteInviteCode()
        {
            inviteCodeInput.text = UniClipboard.GetText();
        }

        public void CancelButton()
        {
            inviteCodeInput.text = "";
            MainMenuManager.INSTANCE.ChangeState(MainMenuManager.MenuState.lobby);
        }
    }    
}

