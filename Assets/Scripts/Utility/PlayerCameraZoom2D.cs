using UnityEngine;

public class PlayerCameraZoom2D : MonoBehaviour
{
    [Header("References")]
    public Camera targetCamera;
    public Rigidbody2D rb;

    [Header("Zoom Settings")]
    public float minZoom = 5f;
    public float maxZoom = 10f;

    [Header("Speed Settings")]
    public float minSpeed = 0f;
    public float maxSpeed = 20f;

    [Header("Smoothness")]
    public float zoomLerpSpeed = 5f;

    void LateUpdate()
    {
        if (targetCamera == null || rb == null)
            return;

        // Get player speed
        float speed = rb.linearVelocity.magnitude;

        // Convert speed into 0-1 value
        float speedPercent = Mathf.InverseLerp(minSpeed, maxSpeed, speed);

        // Calculate desired zoom
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, speedPercent);

        // Smoothly zoom camera
        targetCamera.orthographicSize = Mathf.Lerp(
            targetCamera.orthographicSize,
            targetZoom,
            zoomLerpSpeed * Time.deltaTime
        );
    }
}