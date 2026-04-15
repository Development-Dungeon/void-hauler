using Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utility
{
    /// <summary>
    /// Applies thrust in the world XY plane via <see cref="Rigidbody.AddForce"/>.
    /// Attach to any dynamic <see cref="Rigidbody"/> that should move on a 2D play plane (Z locked by constraints).
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlanarForceMotor : MonoBehaviour
    {
        [SerializeField] [Get] private Rigidbody body;
        [SerializeField] private ForceMode forceMode = ForceMode.Acceleration;
        private Vector2 _thrustInput;
        public PlanarForceMotorData planarForceMotorData;

        /// <summary>
        /// World-space direction on the XY plane. Magnitude is clamped to 0–1.
        /// </summary>
        public void SetPlanarThrustInput(Vector2 worldDirectionXY)
        {
            _thrustInput = Vector2.ClampMagnitude(worldDirectionXY, 1f);
        }
        
        void FixedUpdate()
        {
            if (_thrustInput.sqrMagnitude < 1e-8f)
                return;

            body.AddForce(new Vector3(_thrustInput.x, _thrustInput.y, 0f) * planarForceMotorData.thrustAcceleration, forceMode);

            if (planarForceMotorData.clampPlanarSpeed && planarForceMotorData.MaxPlanarSpeed > 0f)
            {
                var v = body.linearVelocity;
                var planar = new Vector2(v.x, v.y);
                if (planar.sqrMagnitude > planarForceMotorData.MaxPlanarSpeed * planarForceMotorData.MaxPlanarSpeed)
                {
                    planar = planar.normalized * planarForceMotorData.MaxPlanarSpeed;
                    body.linearVelocity = new Vector3(planar.x, planar.y, 0f);
                }
            }

            body.linearVelocity = new Vector3(body.linearVelocity.x, body.linearVelocity.y, 0f);
        }
    }
}
