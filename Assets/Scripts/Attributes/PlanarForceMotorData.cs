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

        public float MaxPlanarSpeed
        {
            get
            {
                if (boostUpgradeEnabled)
                    return _maxPlanarSpeed + boostUpgrade;
                
                return _maxPlanarSpeed;
            }

            set
            {
                _maxPlanarSpeed = value;
            }
        }
        
        public float boostUpgrade = 0f;
        public bool boostUpgradeEnabled = false;

    }
}