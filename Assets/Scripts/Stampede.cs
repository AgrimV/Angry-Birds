using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stampede : MonoBehaviour
{
    [SerializeField] CameraManager _camManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "StampedeEnd")
        {
            _camManager.SwitchToIdleCam();
            Destroy(gameObject);
        }

        Destroy(collision.gameObject);
    }
}
