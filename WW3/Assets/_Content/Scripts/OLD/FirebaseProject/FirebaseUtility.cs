using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

namespace FirebaseProject
{
    public class FirebaseUtility : MonoBehaviour
    {
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
        
        public void Save(string data)
        {
            StartCoroutine(SaveToFirebase(data));
        }

        public void SaveWithPrefix(string prefix, string data)
        {
            StartCoroutine(SaveToFirebaseWithPrefix(prefix, data));
        }

        private IEnumerator SaveToFirebase(string data)
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

        private IEnumerator SaveToFirebaseWithPrefix(string prefix, string data)
        {
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

            Dictionary<string, object> newData = new Dictionary<string, object>();
            newData[prefix] = data;
            
            var task = db.RootReference.Child("users").Child(userId).UpdateChildrenAsync(newData);
            
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
        
        
        public string Load()
        {
            string returnValue = "";
            
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            var dataTask = db.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    Debug.Log("Load successful");
                    
                    //here we get the result from our database.
                    DataSnapshot snap = task.Result;

                    returnValue = snap.GetRawJsonValue();
                }
            });

            return returnValue;
        }
        
        public string LoadWithPrefix(string prefix)
        {
            string returnValue = "";
            
            var db = FirebaseDatabase.DefaultInstance;
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            var dataTask = db.RootReference.Child("users").Child(userId).Child(prefix).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    Debug.Log("Load nickname successful: " + returnValue);
                    
                    //here we get the result from our database.
                    DataSnapshot snap = task.Result;

                    returnValue = snap.GetRawJsonValue();
                }
            });

            return returnValue;
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
                if (message.Contains("The given password is invalid"))
                {
                    MainMenu.instance.ShowPasswordTooShort();
                }
            }
            else
            {
                Debug.Log("Registration Complete");
                
                // Generate initial data in database
                DataWrapper initialData = new DataWrapper();
                initialData.nickname = "";
                initialData.objectName = "";
                initialData.position = Vector3.zero;
                initialData.direction = 0;
                initialData.color = Color.white;

                string jsonData = JsonUtility.ToJson(initialData);
                
                StartCoroutine(SaveToFirebase(jsonData));
                
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
        }
    }
}