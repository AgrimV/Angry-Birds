using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _maxHealth = 5f;
    [SerializeField] float _damageThreshhold = 0.2f;
    [SerializeField] GameObject _deathParticles;
    [SerializeField] AudioClip _deathSound;

    float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    void TakeDamage(float health)
    {
        if (health > _damageThreshhold)
        {
            _currentHealth -= health;
        }

        if (_currentHealth < 0 )
        {
            DieDie();
        }
    }

    void DieDie()
    {
        Vector3 enemyGrave = gameObject.transform.position;

        GameManager.instance.EnemyDown();

        SoundManager.instance.PlayAtPoint(_deathSound, enemyGrave);
        Destroy(gameObject);

        Instantiate(_deathParticles, enemyGrave, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impact = collision.relativeVelocity.magnitude;

        TakeDamage(impact);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage(999f);
    }
}
