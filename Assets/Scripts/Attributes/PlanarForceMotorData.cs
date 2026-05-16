using UnityEngine;

namespace Attributes
{
    [CreateAssetMenu(fileName = "PlanarForceMotorData", menuName = "Attributes/PlanarForceMotorData")]
    public class PlanarForceMotorData: ScriptableObject
    {
        public float thrustAcceleration;
        public bool clampPlanarSpeed;
        [SerializeField]
        private float _maxPlanarSpeed;
        public float MaxPlanarSpeed => _maxPlanarSpeed;
    }
}