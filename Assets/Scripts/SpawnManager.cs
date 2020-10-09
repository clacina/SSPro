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
    private bool _spawnMode = true;

    void Start()
    {
        Debug.Log("Spawn Manager start");
        StartCoroutine(SpawnEmeny());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnEmeny()
    {
        while(_spawnMode)
        {
            if(_enemyPrefab)
            {
                Debug.Log("Spawning...");
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

            switch(Random.Range(0, 100) % 3)
            {
                case 1:
                    newPrefab = _speedPowerupPrefab;
                    break;
                //case 2:
                //    newPrefab = _shieldPowerupPrefab;
                //    break;
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
