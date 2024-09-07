using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotArea : MonoBehaviour
{

    [SerializeField] private LayerMask _slingshotLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsWithinCollider()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Physics2D.OverlapPoint(mousePosition))
        {
            return true;
        }

        return false;
    }
}
