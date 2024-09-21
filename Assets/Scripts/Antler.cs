using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antler : MonoBehaviour
{
    [SerializeField] GameObject _explode;
    [SerializeField] float _explodeRadius = 15.0f;
    [SerializeField] float _explosionForce = 10.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ExplodeEffect();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explodeRadius);

        foreach (Collider2D hit in colliders)
        {
            Debug.Log(hit.name);
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddRelativeForce((hit.transform.position - transform.position).normalized * _explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    void ExplodeEffect()
    {
        Vector3 blastCore = gameObject.transform.position;
        Destroy(gameObject);

        GameObject explosion = Instantiate(_explode, blastCore, Quaternion.identity);
        explosion.transform.localScale = Vector2.one * _explodeRadius;
    }
}
