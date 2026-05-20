using UnityEngine;
using Utility;

namespace player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlanarForceMotor))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Get] [SerializeField] private Rigidbody2D body;
        [Get] [SerializeField] private PlanarForceMotor motor;

        public Rigidbody2D Body => body;

        public void ClearThrust() => motor.SetPlanarThrustInput(Vector2.zero);

        public void SetPlanarThrust(Vector2 worldDirectionOnXY)
        {
            motor.SetPlanarThrustInput(worldDirectionOnXY);
        }

        public void SyncRotation(Quaternion rotation)
        {
            body.MoveRotation(rotation);
            body.angularVelocity = 0;
        }
    }
}
