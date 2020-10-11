using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _spawnY = 7.0f;
    private float _endOfScreen = -5.42f;
    private float _xpos_min = -9.0f;
    private float _xpos_max = 9.16f;

    private UI_Manager um;
    private Animator _animator;

    void Start()
    {
        um = GameObject.FindObjectOfType<UI_Manager>();
        Debug.Assert(um);
        _animator = GetComponent<Animator>();
        Debug.Assert(_animator);
        Spawn();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < _endOfScreen)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        float newXPos = Random.Range(_xpos_min, _xpos_max);
        transform.position = new Vector3(newXPos, _spawnY, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            um.UpdateScore(10);
            Destroy(other.gameObject);
            Die();
        }
    }

    public void Die()
    {
        // Stop Moving
        _speed = 0.0f;
        _animator.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.8f);
    }
}
