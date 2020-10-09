using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private float _playerSpeed = 3.5f, _speedBoostAmount = 2.0f;

    [SerializeField]
    private GameObject _LaserPrefab, _TripleShotLaserPrefab;

    [SerializeField]
    private float _fireRate = 0.15f;

    [SerializeField]
    private bool _UseTripleShot = false, _UseSpeedUpgrade = false, _ShieldsEnabled = false;

    [SerializeField]
    private float _tripleShotDuration = 5.0f, _speedUpgradeDuration = 5.0f, _shieldUpgradeDuration = 8.0f;

    private float _nextFire = 0.0f;
    private SpawnManager _spawnManager = null;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        Debug.Assert(_spawnManager);
        Debug.Assert(_LaserPrefab);
        Debug.Assert(_TripleShotLaserPrefab);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    void FireLaser() { 
        _nextFire = Time.time + _fireRate;
        GameObject laserPrefab = _LaserPrefab;

        if (_UseTripleShot)
        {
            laserPrefab = _TripleShotLaserPrefab;
        }

        GameObject.Instantiate(laserPrefab,
            transform.position + new Vector3(0, 0.9f, 0),
            Quaternion.identity);
    }

    void CalculateMovement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        float movementSpeed = _playerSpeed;
        if(_UseSpeedUpgrade)
        {
            movementSpeed += _speedBoostAmount;
        }

        transform.Translate(direction * movementSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x,
                                         Mathf.Clamp(transform.position.y, -3.81f, 0f),
                                         0);

        if(transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        } else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    /*
     * I'm of the opinion that any collision with another gameobject should
     *  be handled by the object, as opposed to the training material that would
     *  put this in the Enamy class and use a 'GetComponent' to call back to the player.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player hit " + other.tag);
        if(other.tag == "Enemy")
        {
            // decrease life and kill enemy
            Destroy(other.gameObject);
            Damage(1);
        }
    }

    /*
     * Here's our public Damage method that can be called by other objects.
     */
    public void Damage(int amount)
    {
        Die(amount);
    }

    private void Die(int amount)
    {
        Debug.Log("Player Die: " + _lives);
        _lives -= amount;
        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    IEnumerator PowerUptCooldown(PowerUpType powerUpType)
    {
        switch(powerUpType)
        {
            case PowerUpType.TRIPLE_SHOT:
                yield return new WaitForSeconds(_tripleShotDuration);
                _UseTripleShot = false;
                break;
            case PowerUpType.SPEED:
                yield return new WaitForSeconds(_speedUpgradeDuration);
                _UseSpeedUpgrade = false;
                break;
            case PowerUpType.SHIELD:
                yield return new WaitForSeconds(_shieldUpgradeDuration);
                _ShieldsEnabled = false;
                break;
        }
    }

    public void OnPowerUp(PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case PowerUpType.TRIPLE_SHOT:
                _UseTripleShot = true;
                break;
            case PowerUpType.SPEED:
                _UseSpeedUpgrade = true;
                break;
            case PowerUpType.SHIELD:
                _ShieldsEnabled = true;
                break;
        }
        StartCoroutine(PowerUptCooldown(powerUp));
    }
}
