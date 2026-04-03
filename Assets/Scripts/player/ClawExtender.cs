using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Places <see cref="clawTip"/> along the ship forward (same XY direction as thrust: hull <see cref="Transform.up"/>)
/// based on how far the cursor sits in front of the ship on the play plane.
/// Put this on the same object as <see cref="PlayerMovement"/> (the rotating hull), or assign <see cref="shipForward"/>.
/// </summary>
public class ClawExtender : MonoBehaviour
{
    [SerializeField] Camera cameraOverride;
    [Tooltip("Object that rotates with the ship (e.g. Capsule with PlayerMovement). Uses its up = thrust/facing direction.")]
    [SerializeField] Transform shipForward;
    [Tooltip("End of the claw / grab point. World position is driven each frame.")]
    [SerializeField] Transform clawTip;
    [Tooltip("Where the claw starts (e.g. empty at the nose). Defaults to this object.")]
    [SerializeField] Transform mount;
    [Tooltip("Optional: draws a line from mount to tip so the extension is visible.")]
    [SerializeField] LineRenderer extensionLine;
    [Tooltip("World distance along forward at which the claw is fully extended.")]
    [SerializeField] float fullExtendMouseAlong = 5f;
    [Tooltip("How far the tip reaches from the mount at full extension.")]
    [SerializeField] float maxReach = 2.2f;

    Camera Cam => cameraOverride != null ? cameraOverride : Camera.main;

    void Reset()
    {
        mount = transform;
        shipForward = transform;
    }

    Transform Hull => shipForward != null ? shipForward : transform;

    void Update()
    {
        if (clawTip == null || Cam == null || Mouse.current == null)
            return;

        var origin = mount != null ? mount : transform;
        var forward = new Vector2(Hull.up.x, Hull.up.y);
        if (forward.sqrMagnitude < 1e-6f)
            forward = Vector2.up;
        else
            forward.Normalize();

        if (!TryMouseOnPlayPlane(out var world))
            return;

        var shipXY = new Vector2(origin.position.x, origin.position.y);
        var mouseXY = new Vector2(world.x, world.y);
        var along = Vector2.Dot(mouseXY - shipXY, forward);
        along = Mathf.Clamp(along, 0f, Mathf.Max(0f, fullExtendMouseAlong));

        var reach = fullExtendMouseAlong > 0f ? along / fullExtendMouseAlong * maxReach : 0f;
        var f3 = new Vector3(forward.x, forward.y, 0f);
        clawTip.position = origin.position + f3 * reach;

        if (extensionLine != null)
        {
            extensionLine.positionCount = 2;
            extensionLine.SetPosition(0, origin.position);
            extensionLine.SetPosition(1, clawTip.position);
        }
    }

    bool TryMouseOnPlayPlane(out Vector3 point)
    {
        point = default;
        var ray = Cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var plane = new Plane(Vector3.forward, Hull.position);
        if (!plane.Raycast(ray, out float dist))
            return false;
        point = ray.GetPoint(dist);
        return true;
    }
}
