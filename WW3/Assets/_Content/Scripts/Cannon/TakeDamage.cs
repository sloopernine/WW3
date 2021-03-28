using System;
using Data.DataContainers;
using Data.Interfaces;
using UnityEngine;

namespace Cannon
{
    public class TakeDamage : MonoBehaviour, ISendSignal
    {
        private CannonController _cannonController;

        private void Start()
        {
            _cannonController = GetComponent<CannonController>();
        }
        
        public void SendSignal(Signal signal)
        {
            _cannonController.ReceiveSignal(signal);          
        }

        public void Damage()
        {
            Debug.Log("Player: " + _cannonController.playerIndex + " got hit");
            SendSignal(Signal.Die);
        }
    }
}