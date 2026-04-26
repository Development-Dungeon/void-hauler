using System;
using UnityEngine;
using EventChannel.templates;
using VContainer;

namespace Attributes
{
    public class Fuel : MonoBehaviour
    {
        public FuelData templateFuel;
        private FuelData _currentFuel;
        
        public EventChannel<float> fuelChannel;
        public EventChannel<Empty> emptyFuelChannel;

        public float Normalized => _currentFuel.maxFuel > 0f ? _currentFuel.currentFuel / _currentFuel.maxFuel : 0f;
        public bool HasFuel => _currentFuel.currentFuel > 0f;


        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _currentFuel = resolver.ResolveOrDefault<FuelData>();
        }

        private void Awake()
        {
            if (_currentFuel == null)
                _currentFuel = Instantiate(templateFuel);
            
        }

        private void Start()
        {
            SetCurrentFuel(_currentFuel.maxFuel);
        }

        public void RegisterMovement(float metersOnXYPlane)
        {
            if (metersOnXYPlane <= 0f || _currentFuel.currentFuel <= 0f)
                return;
            SetCurrentFuel(Mathf.Max(0f, _currentFuel.currentFuel - metersOnXYPlane * _currentFuel.fuelPerMeter));
            
        }

        private void SetCurrentFuel(float newFuel)
        {
            _currentFuel.currentFuel = newFuel;
            fuelChannel?.Invoke(_currentFuel.currentFuel);
            if(_currentFuel.currentFuel <= 0f)
                emptyFuelChannel?.Invoke(new Empty());
        }

        public void AddFuel(float amount)
        {
            SetCurrentFuel(Mathf.Min(_currentFuel.maxFuel, _currentFuel.currentFuel + amount));
        }

    }
}
