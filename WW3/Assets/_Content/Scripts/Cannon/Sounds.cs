using System;
using Data.Interfaces;
using Data.DataContainers;
using UnityEngine;

namespace Cannon
{
    public class Sounds : MonoBehaviour, IAcceptSignal
    {
        public AudioClip rotateSound;
        public AudioClip setPower;
        public AudioClip fireCannon;
        
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
                
                case Signal.StartSetPower:

                    _audioSource.clip = setPower;
                    _audioSource.Play();
                    
                    break;
                
                case Signal.StopSetPower:

                    _audioSource.Stop();
                    
                    break;
                
                case Signal.FireCannon:

                    _audioSource.PlayOneShot(fireCannon);
                    
                    break;
            }
        }
    }
}