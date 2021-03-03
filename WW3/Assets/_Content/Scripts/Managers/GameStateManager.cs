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
        [SerializeField]private GameState previousGameState;

        public GameState CurrentGameState => currentGameState;
        public GameState PreviousGameState => previousGameState;

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
            if (newGameState == currentGameState)
                return;
            
            previousGameState = currentGameState;
            currentGameState = newGameState;
        
            onChangeGameState?.Invoke(newGameState);
        }
    }
}

