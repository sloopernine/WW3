using System;
using Data.Interfaces;
using Data.Enums;
using UnityEngine;

namespace Cannon
{
    public class Particles : MonoBehaviour, IAcceptSignal
    {
        public ParticleSystem dustCloud;
        public ParticleSystem fireCloud;
        
        private CannonController _cannonController;

        private void Start()
        {
            GetComponent<CannonController>().RegisterReceiver(this);
        }
        
        public void ReceiveSignal(Signal signal)
        {
            switch (signal)
            {
                case Signal.Die:
                    
                    dustCloud.Play();
                    fireCloud.Play();
                    
                    break;
            }
        }
    }
}