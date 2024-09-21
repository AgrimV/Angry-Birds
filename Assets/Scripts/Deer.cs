using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deer : MonoBehaviour
{
    [SerializeField] GameObject _powerUpButton;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            _powerUpButton.SetActive(true);
            Destroy(gameObject);
        }
    }
}
