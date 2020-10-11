using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab=null;
    [SerializeField]
    private GameObject _enemyContainer = null;
    [SerializeField]
    private float _spawnFrequency = 0.4f;
    [SerializeField]
    private GameObject _tripleShotPowerupPrefab = null;
    [SerializeField]
    private GameObject _speedPowerupPrefab = null;
    [SerializeField]
    private GameObject _shieldPowerupPrefab = null;

    private float _spawnY = 7.0f;
    private float _xpos_min = -9.0f;
    private float _xpos_max = 9.16f;
    private bool _spawnMode = false;

    void Start()
    {
    }

    public void StartSpawn()
    {
        _spawnMode = true;
        StartCoroutine(SpawnEmeny());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnEmeny()
    {
        while(_spawnMode)
        {
            if(_enemyPrefab)
            {
                float newXPos = Random.Range(_xpos_min, _xpos_max);

                GameObject newEmeny = Instantiate(_enemyPrefab,
                    new Vector3(newXPos, _spawnY, 0),
                    Quaternion.identity);
                newEmeny.transform.parent = _enemyContainer.transform;

            } else
            {
                Debug.Log("No prefab...");
            }
            yield return new WaitForSeconds(_spawnFrequency);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (_spawnMode)
        {
            float newXPos = Random.Range(_xpos_min, _xpos_max);

            // which power up to spawn?
            GameObject newPrefab = _tripleShotPowerupPrefab;
            PowerUpType newType = (PowerUpType)Random.Range(0, 3);
            switch(newType)
            {
                case PowerUpType.TRIPLE_SHOT:
                    newPrefab = _tripleShotPowerupPrefab;
                    break;
                case PowerUpType.SPEED:
                    newPrefab = _speedPowerupPrefab;
                    break;
                case PowerUpType.SHIELD:
                    newPrefab = _shieldPowerupPrefab;
                    break;
            }

            GameObject powerup = Instantiate(newPrefab,
                new Vector3(newXPos, _spawnY, 0),
                Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _spawnMode = false;
    }
}
