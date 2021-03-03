using System;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Menu;
using UnityEngine;

namespace Data
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager INSTANCE;

        private FirebaseSave _firebaseSave;
        private FirebaseLoad _firebaseLoad;
        private FirebaseUserAuth _firebaseUser;

        public delegate void OnLoadedDelegate(string jsonData);
        public delegate void OnSavedDelegate();

        private FirebaseDatabase _database;

        private void Awake()
        {
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            _firebaseSave = GetComponent<FirebaseSave>();
            _firebaseLoad = GetComponent<FirebaseLoad>();
            _firebaseUser = GetComponent<FirebaseUserAuth>();
            
            _database = FirebaseDatabase.DefaultInstance;
        }
        
        public void LoginUser(string email, string password)
        {
            _firebaseUser.LoginUser(email, password);
        }
        
        public void Logout()
        {
            _firebaseUser.LogoutUser();
        }
        
        public void RegisterUser(string email, string password, string nickname)
        {
            _firebaseUser.RegisterUser(email, password, nickname);
        }
        
        public void SaveData(string path, string data)
        {
            _firebaseSave.SaveData(path, data);
        }
        
        public IEnumerator SaveData(string path, string data, OnSavedDelegate onSavedDelegate = null)
        {
            var dataTask = _database.RootReference.Child(path).SetRawJsonValueAsync(data);
            yield return new WaitUntil(() => dataTask.IsCompleted);

            if (dataTask.Exception != null)
                Debug.LogWarning(dataTask.Exception);

            if (onSavedDelegate != null)
            {
                onSavedDelegate();
            }
        }
        
        public IEnumerator LoadData(string path, OnLoadedDelegate onLoadedDelegate)
        {
            var dataTask = _database.RootReference.Child(path).GetValueAsync();
            yield return new WaitUntil(() => dataTask.IsCompleted);

            if (dataTask.Exception != null)
                Debug.LogWarning(dataTask.Exception);

            string jsonData = dataTask.Result.GetRawJsonValue();

            onLoadedDelegate(jsonData);
        }
    }
}