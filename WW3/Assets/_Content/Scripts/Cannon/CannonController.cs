using System;
using System.Collections;
using System.Collections.Generic;
using Data.Interfaces;
using Data;
using Data.DataContainers;
using Managers;
using UnityEngine;

public class CannonController : MonoBehaviour, IAcceptSignal, ISendSignal
{
    public GameObject cannon;
    public GameObject anchorPoint;
    public GameObject cannonBall;
    
    private SpriteRenderer cannonBaseSprite;
    private SpriteRenderer cannonSprite;
    
    public float firePower = 0;

    public int playerIndex;
    
    private List<IAcceptSignal> receivers = new List<IAcceptSignal>();
    
    // private void OnEnable()
    // {
    //     GameStateManager.INSTANCE.onChangeGameState += OnGameStateChanged;
    // }

    private void Start()
    {
        GameStateManager.INSTANCE.onChangeGameState += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.INSTANCE.onChangeGameState -= OnGameStateChanged;
    }

    public void ReceiveSignal(Signal signal)
    {
        SendSignal(signal);
    }
    
    public void SendSignal(Signal signal)
    {
        foreach (IAcceptSignal receiver in receivers)
        {
            receiver.ReceiveSignal(signal);
        }
    }

    public void RegisterReceiver(IAcceptSignal receiver)
    {
        receivers.Add(receiver);
    }
    
    public void RotateCannon(float value)
    {
        cannon.transform.Rotate(0f, 0f, value, Space.Self);
    }

    public void SetFirepower(float value)
    {
        firePower += value;
    }

    public void SetAngle(float value)
    {
        cannon.transform.eulerAngles = new Vector3(0, 0, value);
    }
    
    public void StartRotateCannon()
    {
        SendSignal(Signal.StartRotate);
    }
    
    public void StopRotateCannon()
    {
        SendSignal(Signal.StopRotate);
    }

    public void FireCannon()
    {
        Vector2 direction = (anchorPoint.transform.position- transform.position).normalized;
        
        GameObject cannonBall = Instantiate(this.cannonBall, anchorPoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = cannonBall.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * firePower, ForceMode2D.Impulse);
    }

    public void EndTurn()
    {
        GameManager.INSTANCE.EndTurn(firePower, cannon.transform.localEulerAngles.z);
    }
    
    private void OnGameStateChanged(GameStateManager.GameState gamestate)
    {
    //     if (DataManager.INSTANCE.GameData.players[DataManager.INSTANCE.GameData.currentTurn].playerID == ActiveUser.INSTANCE._userInfo.userID)
    //     {
    //         if (DataManager.INSTANCE.GameData.currentTurn == playerIndex)
    //         {
    //             buttons.SetActive(true);
    //         }
    //         
    //     }
    //     else
    //     {
    //         buttons.SetActive(false);
    //     }
    }
}
