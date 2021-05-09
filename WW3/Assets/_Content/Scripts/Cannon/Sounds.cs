using System;
using Data.Interfaces;
using Data.Enums;
using OmniLib.Audio;
using UnityEngine;

namespace Cannon
{
    public class Sounds : MonoBehaviour, IAcceptSignal
    {
        public AudioClip rotateSound;
        public AudioClip setPower;
        //public AudioClip fireCannon;
        [SerializeField] private AudioEvent rotateCannon;
        [SerializeField] private AudioEvent fireCannon;
        [SerializeField] private AudioEvent deathExplosion;
        
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
                    
                    rotateCannon.Play(_audioSource);
                    
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

                    fireCannon.Play(_audioSource);
                    
                    break;
                
                case Signal.Die:
                    
                    deathExplosion.Play(_audioSource);
                    
                    break;
            }
        }
    }
}