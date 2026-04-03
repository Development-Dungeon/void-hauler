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
        [SerializeField] private Rigidbody body;
        [FormerlySerializedAs("thrustForce")]
        [SerializeField] private float thrustAcceleration = 35f;
        [SerializeField] private float maxPlanarSpeed = 10f;
        [SerializeField] private ForceMode forceMode = ForceMode.Acceleration;
        [SerializeField] private bool clampPlanarSpeed = true;

        private Vector2 _thrustInput;

        void Awake()
        {
            if (body == null)
                body = GetComponent<Rigidbody>();
        }

        public Rigidbody Body => body;

        /// <summary>
        /// World-space direction on the XY plane. Magnitude is clamped to 0–1.
        /// </summary>
        public void SetPlanarThrustInput(Vector2 worldDirectionXY)
        {
            _thrustInput = worldDirectionXY.sqrMagnitude > 1f ? worldDirectionXY.normalized : worldDirectionXY;
        }

        void FixedUpdate()
        {
            if (_thrustInput.sqrMagnitude < 1e-8f)
                return;

            body.AddForce(new Vector3(_thrustInput.x, _thrustInput.y, 0f) * thrustAcceleration, forceMode);

            if (clampPlanarSpeed && maxPlanarSpeed > 0f)
            {
                var v = body.linearVelocity;
                var planar = new Vector2(v.x, v.y);
                if (planar.sqrMagnitude > maxPlanarSpeed * maxPlanarSpeed)
                {
                    planar = planar.normalized * maxPlanarSpeed;
                    body.linearVelocity = new Vector3(planar.x, planar.y, v.z);
                }
            }

            body.linearVelocity = new Vector3(body.linearVelocity.x, body.linearVelocity.y, 0f);
        }
    }
}
