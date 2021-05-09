using System;
using System.Collections;
using System.Collections.Generic;
using Data.Containers;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Menu;
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager INSTANCE;
        private Data.Enums.GameData _gameData;
        private Data.Enums.UserInfo _activeUser;
        public delegate void OnDataFetched(string jsonData);
        public Data.Enums.GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }

        public Data.Enums.UserInfo ActiveUser
        {
            get => _activeUser;
            set => _activeUser = value;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Destroy(gameObject);
        }
        
        void Start()
        {
            _gameData = new Data.Enums.GameData();
            _activeUser = new Data.Enums.UserInfo();
        }

        public void SaveGameDataToFirebase()
        {
            
        }

        public void RemovePlayerFromGame(string playerID)
        {
            int index = 0;
            
            foreach (var player in _gameData.players)
            {
                if (player.playerID.Contains(playerID))
                {
                    _gameData.players.RemoveAt(index);        
                }

                index++;
            }
        }
        
        private IEnumerator SaveUserDataToFirebase(string data)
        {
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            //puts the json data in the "users/userId" part of the database.
            var task = db.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(data);

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.Log(task.Exception);
            }
            else
            {
                Debug.Log("Save successful");
            }
        }

        public Data.Enums.UserInfo LoadUserInfo()
        {
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            var dataTask = db.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                    string message = task.Exception.InnerExceptions[0].InnerException.Message;
                    MainMenuManager.INSTANCE.DisplayMessage(message, MainMenuManager.MenuState.lobby);
                }
                else
                {
                    Debug.Log("Load successful");
                    
                    //here we get the result from our database.
                    DataSnapshot snap = task.Result;

                    string data = snap.GetRawJsonValue();

                    _activeUser = JsonUtility.FromJson<Data.Enums.UserInfo>(data);
                }
            });

            return _activeUser;
        }
    }
}

