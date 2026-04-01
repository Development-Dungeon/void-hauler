using System;
using Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility;
using VContainer;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Fuel))]
public class PlayerMovement : MonoBehaviour
{
    [Get] [SerializeField] private CharacterController controller;
    [Get] [SerializeField] private Fuel fuel;

    [Inject] private Camera _camera;
    
    private Vector2 _userInput;
    
    [SerializeField] private float thrustSpeed = 2f;
    [SerializeField] private float turnSmoothing = 25f;

    public void OnMove(InputValue value)
    {
        _userInput = value.Get<Vector2>();
    }
    
    void Update()
    {
        ApplyMouseFacing();
        ApplyThrust(_userInput);
    }

    void ApplyThrust(Vector2 userMovementInput)
    {
        if (controller == null || fuel == null)
            return;
        if (userMovementInput == Vector2.zero)
            return;
        if (!fuel.HasFuel)
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
        controller.Move(motion);

        var after = transform.position;
        var traveled = new Vector2(after.x - before.x, after.y - before.y).magnitude;
        if (traveled < 1e-5f)
            traveled = motion.magnitude;
        fuel.RegisterMovement(traveled);
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
        if (_camera == null || Mouse.current == null)
            return false;

        var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
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