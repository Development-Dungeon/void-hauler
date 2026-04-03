using System;
using UnityEngine;
using EventChannel.templates;

namespace Attributes
{
    public class Fuel : MonoBehaviour
    {
        [SerializeField] private float maxFuel = 100f;
        [SerializeField] private float fuelPerMeter = 0.25f;
        [SerializeField] private float currentFuel;

        public EventChannel<float> fuelChannel;

        public float Normalized => maxFuel > 0f ? currentFuel / maxFuel : 0f;
        public bool HasFuel => currentFuel > 0f;

        void Awake()
        {
            currentFuel = maxFuel;
        }

        void Start()
        {
            fuelChannel?.Invoke(currentFuel);
        }

        public void RegisterMovement(float metersOnXYPlane)
        {
            if (metersOnXYPlane <= 0f || currentFuel <= 0f)
                return;
            SetCurrentFuel(Mathf.Max(0f, currentFuel - metersOnXYPlane * fuelPerMeter));
        }

        private void SetCurrentFuel(float newFuel)
        {
            currentFuel = newFuel;
            fuelChannel?.Invoke(currentFuel);
        }

        public void AddFuel(float amount)
        {
            SetCurrentFuel(Mathf.Min(maxFuel, currentFuel + amount));
        }
    }
}
