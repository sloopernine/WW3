using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject cannon;
    public GameObject anchorPoint;
    public GameObject cannonBall;

    public float firePower = 0;

    public int playerIndex;
    
    public AudioClip rotateSound;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        
        GameManager.INSTANCE.UpdateAngle(cannon.transform.localEulerAngles.z);
        GameManager.INSTANCE.UpdateFirepower(firePower);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
