using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    private bool _isFlying = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_isFlying)
        {
            transform.right = _rb.velocity;
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        _isFlying = true;
        _rb.isKinematic = false;
        _rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isFlying = false;
    }
}
