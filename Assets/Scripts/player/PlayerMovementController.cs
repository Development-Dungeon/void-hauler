using UnityEngine;
using Utility;

namespace player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlanarForceMotor))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Get] [SerializeField] private Rigidbody body;
        [Get] [SerializeField] private PlanarForceMotor motor;

        public Rigidbody Body => body;

        public void ClearThrust() => motor.SetPlanarThrustInput(Vector2.zero);

        public void SetPlanarThrust(Vector2 worldDirectionOnXY)
        {
            motor.SetPlanarThrustInput(worldDirectionOnXY);
        }

        public void SyncRotation(Quaternion rotation)
        {
            body.MoveRotation(rotation);
            body.angularVelocity = Vector3.zero;
        }
    }
}
