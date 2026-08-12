using UnityEngine;

public class CanvasRotation : MonoBehaviour
{
    private Quaternion _originalLocalRotation;

    private void Awake()
    {
        _originalLocalRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = _originalLocalRotation;
    }
}