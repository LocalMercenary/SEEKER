using UnityEngine;

public class CanvasBillboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Make the canvas always face the camera
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
