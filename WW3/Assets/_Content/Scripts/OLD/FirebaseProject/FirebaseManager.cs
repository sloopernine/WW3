using System;
using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

namespace FirebaseProject
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        public void RegisterPlayer(string email, string password)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                
                StartCoroutine(RegUser(email, password));
            });
        }

        public void LoginPlayer(string email, string password)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                
                StartCoroutine(SignIn(email, password));
            });
        }
        
        private IEnumerator RegUser(string email, string password)
        {
            Debug.Log("Starting Registration");
            var auth = FirebaseAuth.DefaultInstance;
            var regTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => regTask.IsCompleted);

            if (regTask.Exception != null)
            {
                string message = regTask.Exception.ToString();
                Debug.Log("Message: " + message);
                if (message.Contains("The email address is already in use by another account"))
                {
                    MainMenu.instance.ShowEmailExistWindow();
                }
            }
            else
            {
                Debug.Log("Registration Complete");
                MainMenu.instance.ShowRegisterSuccessWindow();
            }
        }

        private IEnumerator SignIn(string email, string password)
        {
            Debug.Log("Atempting to log in");
            var auth = FirebaseAuth.DefaultInstance;
            var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
                Debug.LogWarning(loginTask.Exception);
            else
                MainMenu.instance.ShowPlayerSettingsWindow();

            StartCoroutine(DataTest(FirebaseAuth.DefaultInstance.CurrentUser.UserId, "TestWrite"));
        }

        private IEnumerator DataTest(string userID, string data)
        {
            Debug.Log("Trying to write data");
            var db = FirebaseDatabase.DefaultInstance;
            var dataTask = db.RootReference.Child("users").Child(userID).SetValueAsync(data);

            yield return new WaitUntil(() => dataTask.IsCompleted);

            if (dataTask.Exception != null)
                Debug.LogWarning(dataTask.Exception);
            else
                Debug.Log("DataTestWrite: Complete");
        }
        
        private void SaveToFirebase(string data)
        {
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            //puts the json data in the "users/userId" part of the database.
            db.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(data);
        }
        
        private void LoadFromFirebase()
        {
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            var dataTask = db.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }

                //here we get the result from our database.
                DataSnapshot snap = task.Result;

                //And send the json data to a function that can update our game.
                //LoadState(snap.GetRawJsonValue());
            });
        }
    }
}

