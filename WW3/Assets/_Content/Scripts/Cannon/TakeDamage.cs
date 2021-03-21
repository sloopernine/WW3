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
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Shells")
            {
                SendSignal(Signal.Die);
            }
        }
    }
}