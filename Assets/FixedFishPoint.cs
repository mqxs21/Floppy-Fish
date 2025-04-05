using UnityEngine;

public class FixedPushPoint : MonoBehaviour
{
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalPosition;

    void Start()
    {
        initialLocalRotation = transform.localRotation;
        initialLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        // Maintain original local rotation regardless of parent's rotation
        transform.localRotation = initialLocalRotation;
        // Maintain original local position regardless of parent's position
        transform.localPosition = initialLocalPosition;
    }
}
