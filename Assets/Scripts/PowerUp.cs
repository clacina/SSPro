using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    TRIPLE_SHOT, SPEED, SHIELD
};

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private PowerUpType _powerupType = PowerUpType.TRIPLE_SHOT;

    [SerializeField]
    private float _speed = 3.5f;
    private float _endOfScreen = -5.42f;

    private Player _player = null;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        Debug.Assert(_player);
        Debug.Log("Found Player");
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _endOfScreen)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
            Debug.Log("Calling player");
            _player.OnPowerUp(_powerupType);
        }
    }
}