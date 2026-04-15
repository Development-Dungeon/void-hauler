using Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using VContainer;

namespace player
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(Fuel))]
    public class PlayerMovement : MonoBehaviour
    {
        [Get] [SerializeField] private PlayerMovementController controller;
        [Get] [SerializeField] private Fuel fuel;

        private Camera _camera;

        private Vector2 _userInput;
        private Quaternion _rotationTarget;
        private Vector2 _lastPositionForFuel;

        public MovementData movementDataTemplate; 
        private MovementData _currentMovementData;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _camera = resolver.Resolve<Camera>();
            _currentMovementData = resolver.ResolveOrDefault<MovementData>();
        }

        void Awake()
        {
            if (_currentMovementData == null)
                _currentMovementData = Instantiate(movementDataTemplate);
            
            _rotationTarget = transform.rotation;
            _lastPositionForFuel = new Vector2(transform.position.x, transform.position.y);
        }

        public void OnMove(InputValue value)
        {
            _userInput = value.Get<Vector2>();
        }

        void Update()
        {
            ApplyMouseFacing();
            ApplyThrust(_userInput);
            ApplyCameraFollow();
        }

        void FixedUpdate()
        {
            controller.SyncRotation(_rotationTarget);
        }

        void LateUpdate()
        {
            var xy = new Vector2(transform.position.x, transform.position.y);
            if (_userInput != Vector2.zero && fuel != null && fuel.HasFuel)
            {
                var traveled = Vector2.Distance(xy, _lastPositionForFuel);
                if (traveled > 1e-5f)
                    fuel.RegisterMovement(traveled);
            }

            _lastPositionForFuel = xy;
        }

        private void ApplyCameraFollow()
        {
            var currentPlayerPosition = transform.position;
            _camera.transform.position = new Vector3(currentPlayerPosition.x, currentPlayerPosition.y, _camera.transform.position.z);
        }

        void ApplyThrust(Vector2 userMovementInput)
        {
            if (fuel == null || !fuel.HasFuel || userMovementInput == Vector2.zero)
            {
                controller.ClearThrust();
                return;
            }

            var aim = new Vector2(transform.up.x, transform.up.y);
            if (aim.sqrMagnitude < 1e-6f)
                aim = Vector2.up;
            else
                aim.Normalize();

            var strafe = new Vector2(-aim.y, aim.x);
            var move = strafe * userMovementInput.x + aim * userMovementInput.y;
            if (move.sqrMagnitude > 1f)
                move.Normalize();
            
            controller.SetPlanarThrust(move);
        }

        void ApplyMouseFacing()
        {
            if (!TryGetMouseAimInPlayPlane(out var dir))
                return;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    
            Quaternion target = Quaternion.Euler(0, 0, angle - 90f);

            _rotationTarget = Quaternion.Slerp(
                _rotationTarget,
                target,
                1f - Mathf.Exp(-_currentMovementData.turnSmoothing * Time.deltaTime));
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
}
