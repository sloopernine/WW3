using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Menu;
using UnityEngine;

namespace Data
{
    public class FirebaseUserAuth : MonoBehaviour
    {
        #region Login & Logout

        public void LoginUser(string email, string password)
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
            {
                ActiveUser.INSTANCE.LoadUserInfo();
                MainMenuManager.INSTANCE.ChangeState(MainMenuManager.MenuState.lobby);
            }
        }

        public void LogoutUser()
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
            
            MainMenuManager.INSTANCE.DisplayMessage("You are now signed out", MainMenuManager.MenuState.login);
        }

        #endregion

        #region Register

        public void RegisterUser(string email, string password, string nickname)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null)
                {
                    Debug.LogError(task.Exception);
                }
                
                StartCoroutine(RegUser(email, password, nickname));
            });
        }
        
        private IEnumerator RegUser(string email, string password, string nickname)
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
                Enums.UserInfo userInfo = new Enums.UserInfo();
                userInfo.userID = auth.CurrentUser.UserId;
                userInfo.nickname = nickname;
                ActiveUser.INSTANCE._userInfo = userInfo;
                ActiveUser.INSTANCE.SaveUserInfo();

                Debug.Log("Registration Complete");
                MainMenuManager.INSTANCE.DisplayMessage("Registration Complete", MainMenuManager.MenuState.login);
            }
        }

        #endregion
    }
}