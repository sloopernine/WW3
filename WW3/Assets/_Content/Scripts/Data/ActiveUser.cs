using System.Collections;
using Data.DataContainers;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Menu;
using UnityEngine;

namespace Data
{
    public class ActiveUser : MonoBehaviour
    {
        public static ActiveUser INSTANCE;

        public Data.DataContainers.UserInfo _userInfo;
        
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Destroy(this.gameObject);
        }

        private void Start()
        {
            _userInfo = new Data.DataContainers.UserInfo();
        }

        public void SaveUserInfo()
        {
            string data = JsonUtility.ToJson(_userInfo);
            
            StartCoroutine(SaveUserDataToFirebase(data));
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

        public void RemoveActiveGame(string gameName)
        {
            int index = 0;
            
            foreach (var activeGame in _userInfo.activeGames)
            {
                if (activeGame.gameID.Contains(gameName))
                {
                    _userInfo.activeGames.RemoveAt(index);    
                }

                index++;
            }

            SaveUserInfo();
        }
        
        public Data.DataContainers.UserInfo LoadUserInfo()
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

                    _userInfo = JsonUtility.FromJson<UserInfo>(data);
                }
            });

            return _userInfo;
        }
    }
}