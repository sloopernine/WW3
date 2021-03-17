using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Managers;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject cannon;
    public GameObject anchorPoint;
    public GameObject cannonBall;
    
    public ParticleSystem dustCloud;
    public ParticleSystem fireCloud;

    private SpriteRenderer cannonBaseSprite;
    private SpriteRenderer cannonSprite;
    
    public float firePower = 0;

    public int playerIndex;
    
    public AudioClip rotateSound;
    private AudioSource _audioSource;

    public GameObject buttons;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        cannonBaseSprite = GetComponent<SpriteRenderer>();
        cannonSprite = cannon.GetComponent<SpriteRenderer>();
        
        GameManager.INSTANCE.UpdateAngle(cannon.transform.localEulerAngles.z);
        GameManager.INSTANCE.UpdateFirepower(firePower);
    }

    private void OnEnable()
    {
        GameStateManager.INSTANCE.onChangeGameState += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.INSTANCE.onChangeGameState -= OnGameStateChanged;
    }

    public void RotateCannon(float value)
    {
        cannon.transform.Rotate(0f, 0f, value, Space.Self);
        GameManager.INSTANCE.UpdateAngle(cannon.transform.localEulerAngles.z);
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
        _audioSource.clip = rotateSound;
        _audioSource.Play();
    }
    
    public void StopRotateCannon()
    {
        _audioSource.Stop();
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
        buttons.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Shells")
        {
            Die();    
        }
    }

    private void Die()
    {
        Debug.Log("Player " + playerIndex + " died");
        DataManager.INSTANCE.GameData.players[playerIndex].isAlive = false;
        dustCloud.Play();
        fireCloud.Play();

        cannonSprite.enabled = false;
        cannonBaseSprite.enabled = false;
        
        //TODO Somehow save that this player has died to database
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
