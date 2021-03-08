using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager instance;

        public enum GameState
        {
            PlayersTurn,
            OpponentTurn,
            GameOver,
            Victory
        }

        [SerializeField]private GameState currentGameState;

        public GameState CurrentGameState => currentGameState;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
    
        public Action<GameStateManager.GameState> onChangeGameState;
        public void ChangeGameState(GameState newGameState)
        {
            currentGameState = newGameState;
            onChangeGameState?.Invoke(newGameState);
        }
    }
}

