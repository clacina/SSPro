using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _playerSpeed = 3.5f;
    [SerializeField]
    private GameObject _LaserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = 0.0f;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
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
        GameObject.Instantiate(_LaserPrefab,
            transform.position + new Vector3(0, 0.9f, 0),
            Quaternion.identity);
    }

    void CalculateMovement()
    {
        float horizontal_input = Input.GetAxis("Horizontal");
        float vertical_input = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal_input, vertical_input, 0);

        transform.Translate(direction * _playerSpeed * Time.deltaTime);

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
    private void OnTriggerEnter(Collider other)
    {
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
        _lives -= amount;
        if (_lives <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
