using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Mouse aims the ship in the XY plane; WASD thrusts relative to aim. Fuel drains while moving.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerFuel))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerFuel _fuel;

    [SerializeField] private Camera _camera;
    [FormerlySerializedAs("moveSpeed")]
    [SerializeField] private float thrustSpeed = 2f;
    [SerializeField] private float turnSmoothing = 25f;

    void Awake()
    {
        // Broken scenes can have duplicate PlayerMovement; only the first component stays enabled.
        var allMovement = GetComponents<PlayerMovement>();
        if (allMovement.Length > 0 && allMovement[0] != this)
        {
            enabled = false;
            return;
        }

        _controller = GetComponent<CharacterController>();
        _fuel = GetComponent<PlayerFuel>();
        if (_fuel == null)
            _fuel = gameObject.AddComponent<PlayerFuel>();

        if (_controller == null)
            Debug.LogError("PlayerMovement requires a CharacterController.", this);
    }

    void Update()
    {
        var moveInput = ReadWasd();
        ApplyMouseFacing();
        ApplyThrust(moveInput);
    }

    static Vector2 ReadWasd()
    {
        var k = Keyboard.current;
        if (k == null)
            return Vector2.zero;

        float x = 0f, y = 0f;
        if (k.aKey.isPressed || k.leftArrowKey.isPressed) x -= 1f;
        if (k.dKey.isPressed || k.rightArrowKey.isPressed) x += 1f;
        if (k.sKey.isPressed || k.downArrowKey.isPressed) y -= 1f;
        if (k.wKey.isPressed || k.upArrowKey.isPressed) y += 1f;

        var v = new Vector2(x, y);
        if (v.sqrMagnitude > 1f)
            v.Normalize();
        return v;
    }

    void ApplyThrust(Vector2 userMovementInput)
    {
        if (_controller == null || _fuel == null)
            return;
        if (userMovementInput == Vector2.zero)
            return;
        if (!_fuel.HasFuel)
            return;

        var aim = new Vector2(transform.up.x, transform.up.y);
        if (aim.sqrMagnitude < 1e-6f)
            aim = Vector2.up;
        else
            aim.Normalize();

        var strafe = new Vector2(-aim.y, aim.x);
        var move = strafe * userMovementInput.x + aim * userMovementInput.y;
        if (move.sqrMagnitude > 1f)
            move.Normalize();

        var motion = new Vector3(move.x, move.y, 0f) * (thrustSpeed * Time.deltaTime);
        var before = transform.position;
        _controller.Move(motion);

        var after = transform.position;
        var traveled = new Vector2(after.x - before.x, after.y - before.y).magnitude;
        if (traveled < 1e-5f)
            traveled = motion.magnitude;
        _fuel.RegisterMovement(traveled);
    }

    void ApplyMouseFacing()
    {
        if (!TryGetMouseAimInPlayPlane(out var dir))
            return;

        var forward = Vector3.Cross(Vector3.up, dir);
        if (forward.sqrMagnitude < 1e-6f)
            forward = Vector3.Cross(Vector3.forward, dir);
        if (forward.sqrMagnitude < 1e-6f)
            return;
        forward.Normalize();

        var target = Quaternion.LookRotation(forward, dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            1f - Mathf.Exp(-turnSmoothing * Time.deltaTime));
    }

    bool TryGetMouseAimInPlayPlane(out Vector3 dir)
    {
        dir = default;
        var cam = _camera != null ? _camera : Camera.main;
        if (cam == null || Mouse.current == null)
            return false;

        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        var plane = new Plane(Vector3.forward, transform.position);
        if (!plane.Raycast(ray, out float distance))
            return false;

        var point = ray.GetPoint(distance);
        dir = point - transform.position;
        dir.z = 0f;
        if (dir.sqrMagnitude < 1e-6f)
            return false;
        dir.Normalize();
        return true;
    }
}

/// <summary>
/// Fuel drains when the player moves (see PlayerMovement). At zero fuel, thrust stops.
/// </summary>
[DisallowMultipleComponent]
public class PlayerFuel : MonoBehaviour
{
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float fuelPerMeter = 1f;

    private float _currentFuel;

    public float Normalized => maxFuel > 0f ? _currentFuel / maxFuel : 0f;
    public bool HasFuel => _currentFuel > 0f;

    void Awake()
    {
        _currentFuel = maxFuel;
    }

    public void RegisterMovement(float metersOnXYPlane)
    {
        if (metersOnXYPlane <= 0f || _currentFuel <= 0f)
            return;
        _currentFuel = Mathf.Max(0f, _currentFuel - metersOnXYPlane * fuelPerMeter);
    }

    public void AddFuel(float amount)
    {
        _currentFuel = Mathf.Min(maxFuel, _currentFuel + amount);
    }
}
