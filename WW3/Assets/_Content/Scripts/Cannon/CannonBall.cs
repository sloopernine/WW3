using System;
using System.Collections;
using System.Collections.Generic;
using Cannon;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;

    public int creatorPlayerIndex;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        //TODO Kill cannonball if it takes too long before hitting something, for example falling outside of world
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<TakeDamage>().Damage();
        }
        else
        {
            //TODO Play particle and sound for non player explosion here
        }
        
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _spriteRenderer.enabled = false;

        if (creatorPlayerIndex == GameManager.INSTANCE.localPlayer.playerIndex)
        {
            GameManager.INSTANCE.localPlayer.EndTurn();
        }
        
        StartCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitUntil(() => _trailRenderer.positionCount <= 0);
        Destroy(this.gameObject);
    }
}
