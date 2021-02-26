using System;
using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager INSTANCE;

        #region Login Panel

        [Header("Login Panel")]
        public GameObject loginPanel;
        public TMP_InputField loginEmailInput;
        public TMP_InputField loginPasswordInput;
        public Toggle toggleRememberMe;

        #endregion

        #region Register Panel

        [Header("Register Panel")]
        public GameObject registerPanel;
        public TMP_InputField registerNicknameInput;
        public TMP_InputField registerEmailInput;
        public TMP_InputField registerPasswordInput;

        #endregion

        #region Message Panel

        [Header("Message Panel")]
        public GameObject messagePanel;
        public TMP_Text messageText;

        #endregion

        #region Create Join Panel

        [Header("Create Join Panel")]
        public GameObject createJoinPanel;

        #endregion
        
        public enum MenuState
        {
            login,
            register,
            message,
            createJoin
        }
        private MenuState currentMenuState;
        public MenuState CurrentPanelState => currentMenuState;

        private MenuState lastMenuState;
        public MenuState LastMenuState => lastMenuState;

        private void Awake()
        {
            if (INSTANCE == null)
                INSTANCE = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            ChangeState(MenuState.login);
            
            int remember = PlayerPrefs.GetInt("remember", 0);

            if (remember != 0)
            {
                toggleRememberMe.isOn = true;
                loginEmailInput.text = PlayerPrefs.GetString("email", String.Empty);
                loginPasswordInput.text = PlayerPrefs.GetString("password", String.Empty);
            }
            else
            {
                toggleRememberMe.isOn = false;
            }
        }
        
        public void LoginButton()
        {
            if (toggleRememberMe.isOn)
            {
                PlayerPrefs.SetString("email", loginEmailInput.text);
                PlayerPrefs.SetString("password", loginPasswordInput.text);
                PlayerPrefs.SetInt("remember", 1);
            }
            else
            {
                PlayerPrefs.SetString("", loginEmailInput.text);
                PlayerPrefs.SetString("", loginPasswordInput.text);
                PlayerPrefs.SetInt("remember", 0);
            }
            
            FirebaseManager.INSTANCE.LoginPlayer(loginEmailInput.text, loginPasswordInput.text);
        }

        public void RegisterPanelButton()
        {
            ChangeState(MainMenuManager.MenuState.register);
        }

        public void RegisterButton()
        {
            FirebaseManager.INSTANCE.RegisterPlayer(registerEmailInput.text, registerPasswordInput.text);
        }

        public void GoToLoginPanel()
        {
            ChangeState(MenuState.login);
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        public void DisplayMessage(string message, MenuState fallbackState)
        {
            if (message.Contains("password is invalid"))
            {
                message = "Password too short!";
            }
            
            messageText.text = message;

            MessagePanel msgPanel = messagePanel.GetComponent<MessagePanel>();
            msgPanel.fallbackState = fallbackState;
            ChangeState(MenuState.message);
        }

        public void ChangeState(MenuState newState)
        {
            if (currentMenuState == newState)
                return;

            if (newState == MenuState.login)
            {
                loginPanel.SetActive(true);
                registerPanel.SetActive(false);
                messagePanel.SetActive(false);
                createJoinPanel.SetActive(false);

                if (lastMenuState == MenuState.register)
                {
                    registerNicknameInput.text = "";
                    registerEmailInput.text = "";
                    registerPasswordInput.text = "";
                }
            }
            
            if (newState == MenuState.register)
            {
                loginPanel.SetActive(false);
                registerPanel.SetActive(true);
                messagePanel.SetActive(false);
                createJoinPanel.SetActive(false);
            }

            if (newState == MenuState.message)
            {
                loginPanel.SetActive(false);
                registerPanel.SetActive(false);
                messagePanel.SetActive(true);
                createJoinPanel.SetActive(false);
            }
            
            if (newState == MenuState.createJoin)
            {
                loginPanel.SetActive(false);
                registerPanel.SetActive(false);
                messagePanel.SetActive(false);
                createJoinPanel.SetActive(true);
            }

            lastMenuState = currentMenuState;
            currentMenuState = newState;
        }
    }
}