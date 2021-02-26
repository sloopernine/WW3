using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _spriteRenderer.enabled = false;
        StartCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitUntil(() => _trailRenderer.positionCount <= 0);
        Destroy(this.gameObject);
    }
}
