using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static Controls _gameControls;

    public static Vector2 MousePosition;
    public static bool WasPressedThisFrame;
    public static bool WasReleasedThisFrame;
    public static bool IsPressed;

    void Awake()
    {
        _gameControls = new Controls();
    }

    private void OnEnable()
    {
        _gameControls.Enable();
    }

    private void OnDisable()
    {
        _gameControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = _gameControls.GameControls.InputPosition.ReadValue<Vector2>();
        WasPressedThisFrame = _gameControls.GameControls.Input.WasPressedThisFrame();
        WasReleasedThisFrame = _gameControls.GameControls.Input.WasReleasedThisFrame();
        IsPressed = _gameControls.GameControls.Input.IsPressed();
    }
}
