using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 19.0f;

    [SerializeField]
    private GameObject _exposionPrefab = null;

    private SpawnManager _spawnManager = null;

    void Start()
    {
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        Debug.Assert(_spawnManager);
        Debug.Assert(_exposionPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        // rotate on 'z' axis 3 meters per second
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            // trigger animation and destroy
            GameObject.Instantiate(_exposionPrefab,
                transform.position,
                Quaternion.identity);
            Destroy(this.gameObject, 1.8f);
            _spawnManager.StartSpawn();
        }
    }
}
