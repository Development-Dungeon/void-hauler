using System;
using UnityEngine;
using EventChannel.templates;
using Upgrades;
using VContainer;

namespace Attributes
{
    public class Fuel : MonoBehaviour
    {
        public FuelData templateFuel;
        private FuelData _currentFuel;
        
        public EventChannel<float> fuelChannel;
        public EventChannel<Empty> emptyFuelChannel;

        [Header("Upgrades")]
        [Space]
        public PlayerUpgradeController playerUpgradeController;
        [Space]
        public float fuelTakeUpgradeIncrease = 25f;
        public bool fuelTakeUpgradeEnabled;
        
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
            
            if(playerUpgradeController != null)
                playerUpgradeController.fuelUpgrade.Action += EnableUpgrade;
        }

        private void EnableUpgrade()
        {
            fuelTakeUpgradeEnabled = true;
        }


        private void Start()
        {
            SetCurrentFuel(GetMaxFuel());
        }

        private float GetMaxFuel()
        {
           if(!fuelTakeUpgradeEnabled) 
               return _currentFuel.maxFuel;
           
           return _currentFuel.maxFuel + fuelTakeUpgradeIncrease;
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
            SetCurrentFuel(Mathf.Min(GetMaxFuel(), _currentFuel.currentFuel + amount));
        }

        private void OnDestroy()
        {
            if(playerUpgradeController != null)
                playerUpgradeController.fuelUpgrade.Action -= EnableUpgrade;
        }
    }
}
