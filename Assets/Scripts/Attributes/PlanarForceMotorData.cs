using UnityEngine;

namespace Attributes
{
    [CreateAssetMenu(fileName = "PlanarForceMotorData", menuName = "Attributes/PlanarForceMotorData")]
    public class PlanarForceMotorData: ScriptableObject
    {
        public float thrustAcceleration;
        public bool clampPlanarSpeed;
        public float maxPlanarSpeed;
        
        // how to include upgrades

    }
}