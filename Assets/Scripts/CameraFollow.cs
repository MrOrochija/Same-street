using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 2, -10);

    [HideInInspector] public bool active = true;

    private Transform cameraTransform;

    void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null && active)
        {
            cameraTransform.position = transform.position + offset;
            cameraTransform.LookAt(transform);
        }
    }
}