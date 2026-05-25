using System;
using Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Upgrades;

namespace Utility
{
    /// <summary>
    /// Applies thrust in the world XY plane via <see cref="Rigidbody.AddForce"/>.
    /// Attach to any dynamic <see cref="Rigidbody"/> that should move on a 2D play plane (Z locked by constraints).
    /// </summary>
    //[RequireComponent(typeof(Rigidbody))]
    public class PlanarForceMotor : MonoBehaviour
    {
        [SerializeField] [Get] private Rigidbody2D body;
        [SerializeField] private ForceMode2D forceMode = ForceMode2D.Impulse;
        private Vector2 _thrustInput;
        public PlanarForceMotorData planarForceMotorData;
        
        [Header("Upgrades")]
        [Space]
        public PlayerUpgradeController playerUpgradeController;
        [Space]
        public bool speedUpgradePurchased;
        public float speedUpgradeIncrease = 10f;
        [Space]
        public bool boostUpgradePurchased;
        public bool boostUpgradeEnabled;
        public float boostUpgradeIncrease = 10f;
        [Space]
        public InputAction inputAction;


        private void OnEnable()
        {
            inputAction.Enable();
            
            // subscribe to the events
            inputAction.performed += ctx => boostUpgradeEnabled = true;
            inputAction.canceled += ctx => boostUpgradeEnabled = false;
        }

        private void OnDisable()
        {
            inputAction.Disable();
        }

        private void Awake()
        {
            if (playerUpgradeController != null)
            {
                playerUpgradeController.thrusterUpgrade.Action += EnableThrusterUpgrade;
                playerUpgradeController.boostUpgrade.Action += EnableBoostUpgrade;
            }
        }

        private void EnableBoostUpgrade()
        {
            boostUpgradePurchased = true;
        }

        private void EnableThrusterUpgrade()
        {
            speedUpgradePurchased = true;
        }


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

            body.AddForce(new Vector2(_thrustInput.x, _thrustInput.y) * planarForceMotorData.thrustAcceleration, forceMode);


            if (planarForceMotorData.clampPlanarSpeed && GetMaxSpeed() > 0f)
            {
                var v = body.linearVelocity;
                var planar = new Vector2(v.x, v.y);
                if (planar.sqrMagnitude > GetMaxSpeed() * GetMaxSpeed())
                {
                    planar = planar.normalized * GetMaxSpeed();
                    body.linearVelocity = new Vector3(planar.x, planar.y, 0f);
                }
            }

            body.linearVelocity = new Vector3(body.linearVelocity.x, body.linearVelocity.y, 0f);
        }

        private float GetMaxSpeed()
        {
            var bonusSpeed = 0f;
            
            bonusSpeed += speedUpgradePurchased ? speedUpgradeIncrease : 0f;
            bonusSpeed += boostUpgradePurchased && boostUpgradeEnabled ? boostUpgradeIncrease : 0f;
            
            return planarForceMotorData.MaxPlanarSpeed + bonusSpeed;
        }

        private void OnDestroy()
        {
            if (playerUpgradeController != null)
            {
                playerUpgradeController.thrusterUpgrade.Action += EnableThrusterUpgrade;
                playerUpgradeController.boostUpgrade.Action += EnableBoostUpgrade;
            }
        }
    }
}
