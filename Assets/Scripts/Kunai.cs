using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Kunai : MonoBehaviour
{
    [SerializeField] float _destroyTime = 1.0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }
}
