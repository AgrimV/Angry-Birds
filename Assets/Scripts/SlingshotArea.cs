using UnityEngine;

public class SlingshotArea : MonoBehaviour
{

    [SerializeField] private LayerMask _slingshotLayerMask;

    public bool IsWithinCollider()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        if (Physics2D.OverlapPoint(mousePosition, _slingshotLayerMask))
        {
            return true;
        }

        return false;
    }
}
