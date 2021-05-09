using System;
using Data;
using Data.Enums;
using Data.Interfaces;
using Managers;
using UnityEngine;

namespace Cannon
{
    public class Die : MonoBehaviour, IAcceptSignal
    {
        public GameObject cannon;
        
        private SpriteRenderer cannonBaseSprite;
        private SpriteRenderer cannonSprite;

        private CannonController _cannonController;
        
        private void Start()
        {
            cannonBaseSprite = GetComponent<SpriteRenderer>();
            cannonSprite = cannon.GetComponent<SpriteRenderer>();
            _cannonController = GetComponent<CannonController>();
            
            GetComponent<CannonController>().RegisterReceiver(this);
        }
        
        public void ReceiveSignal(Signal signal)
        {
            switch (signal)
            {
                case Signal.Die:
                    
                    Debug.Log("Player " + _cannonController.playerIndex + " died");

                    int playerIndex = _cannonController.playerIndex;
                    int activeGameIndex = ActiveUser.INSTANCE.GetIndexByGameID(DataManager.INSTANCE.GameData.gameID);

                    ActiveUser.INSTANCE._userInfo.activeGames[activeGameIndex].playerList[playerIndex].isAlive = false;
                    ActiveUser.INSTANCE.SaveUserInfo();

                    DataManager.INSTANCE.GameData.players[playerIndex].isAlive = false;
                    
                    cannonSprite.enabled = false;
                    cannonBaseSprite.enabled = false;
        
                    //TODO Somehow save that this player has died to database
                    
                    break;
            }
        }
    }
}