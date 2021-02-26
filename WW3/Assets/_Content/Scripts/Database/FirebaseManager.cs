using System;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using FirebaseProject;
using Menu;
using UnityEngine;

namespace Database
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager INSTANCE;

        private void Awake()
        {
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Destroy(this);
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
        
        private IEnumerator SignIn(string email, string password)
        {
            Debug.Log("Atempting to log in");
            var auth = FirebaseAuth.DefaultInstance;
            var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
                Debug.LogWarning(loginTask.Exception);
            else
                MainMenuManager.INSTANCE.ChangeState(MainMenuManager.MenuState.createJoin);
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
        
        private IEnumerator RegUser(string email, string password)
        {
            Debug.Log("Starting Registration");
            var auth = FirebaseAuth.DefaultInstance;
            var regTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => regTask.IsCompleted);

            if (regTask.Exception != null)
            {
                string message = regTask.Exception.InnerExceptions[0].InnerException.Message;
                Debug.Log("Message: " + message);
                
                MainMenuManager.INSTANCE.DisplayMessage(message, MainMenuManager.MenuState.register);
            }
            else
            {
                Debug.Log("Registration Complete");
                MainMenuManager.INSTANCE.DisplayMessage("Registration Complete", MainMenuManager.MenuState.login);
            }
        }

        public void SignOut()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }

                var auth = FirebaseAuth.DefaultInstance;
                
                auth.SignOut();
            });
            
            MainMenuManager.INSTANCE.ChangeState(MainMenuManager.MenuState.login);
        }
    }
}