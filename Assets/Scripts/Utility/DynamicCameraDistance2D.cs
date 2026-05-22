using UnityEngine;

public class DynamicCameraDistance2D : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D playerRb;

    [Header("Zoom Settings")]
    public float minZoom = 5f;
    public float maxZoom = 10f;

    [Header("Speed Settings")]
    public float minSpeed = 0f;
    public float maxSpeed = 20f;

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 0, -10);

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null || playerRb == null || cam == null)
            return;

        // Follow player
        transform.position = player.position + offset;

        // Get player speed
        float speed = playerRb.linearVelocity.magnitude;

        // Convert speed to 0-1 range
        float speedPercent = Mathf.InverseLerp(minSpeed, maxSpeed, speed);

        // Determine target zoom
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, speedPercent);

        // Smooth zoom
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            smoothSpeed * Time.deltaTime
        );
    }
}