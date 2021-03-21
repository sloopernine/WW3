using System;
using Data.Interfaces;
using Data.DataContainers;
using UnityEngine;

namespace Cannon
{
    public class Particles : MonoBehaviour, IAcceptSignal
    {
        public ParticleSystem dustCloud;
        public ParticleSystem fireCloud;
        
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