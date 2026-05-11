using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    [Header("World Bounds")]
    public float minX = -164f;
    public float maxX = 164f;
    public float minY = -164f;
    public float maxY = 164f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}