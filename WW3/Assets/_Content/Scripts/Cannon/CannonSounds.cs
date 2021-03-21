using System;
using Data.Interfaces;
using Data.DataContainers;
using UnityEngine;

namespace Cannon
{
    public class CannonSounds : MonoBehaviour, IAcceptSignal
    {
        public AudioClip rotateSound;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            
            GetComponent<CannonController>().RegisterReceiver(this);
        }

        public void ReceiveSignal(Signal signal)
        {
            switch (signal)
            {
                case Signal.StartRotate:
                    
                    _audioSource.clip = rotateSound;
                    _audioSource.Play();
                    
                    break;
                
                case Signal.StopRotate:
                    
                    _audioSource.Stop();
                    
                    break;
            }
        }
    }
}