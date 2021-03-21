using System;
using Data.DataContainers;
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
        }
        
        public void ReceiveSignal(Signal signal)
        {
            switch (signal)
            {
                case Signal.Die:
                    
                    Debug.Log("Player " + _cannonController.playerIndex + " died");
                    
                    DataManager.INSTANCE.GameData.players[_cannonController.playerIndex].isAlive = false;
                    
                    cannonSprite.enabled = false;
                    cannonBaseSprite.enabled = false;
        
                    //TODO Somehow save that this player has died to database
                    
                    break;
            }
        }
    }
}