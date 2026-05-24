using System;
using EventChannel.concrete;
using EventChannel.templates;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace Attributes
{

    public class Health : MonoBehaviour
    {
        public HealthData templateHealthData;
        private HealthData _currentHealthData;
    
        public FloatEventChannel healthPercentChannel;
        public EventChannel<Health> onDeathChannel;
        public EventChannel<Health> onAliveChannel;
        private bool _managedObject = false;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _currentHealthData = resolver.ResolveOrDefault<HealthData>();
            _managedObject = true;
        }


        private void Awake()
        {
            if(_currentHealthData == null)
                _currentHealthData = Instantiate(templateHealthData);

            if (!_managedObject)
            {
                if(healthPercentChannel != null)
                    healthPercentChannel = Instantiate(healthPercentChannel);
                if(onDeathChannel != null)
                    onDeathChannel = Instantiate(onDeathChannel);
                if(onAliveChannel != null)
                    onAliveChannel = Instantiate(onAliveChannel);
            }
            
        }

        private void Start()
        {
            SetCurrentHealth(_currentHealthData.maxHealth);
        }

        private void SetCurrentHealth(float toBeCurrentHealth)
        {
            var previousState = IsDead;
        
            _currentHealthData.currentHealth = toBeCurrentHealth;

            var newState = IsDead;

            if (previousState != newState) 
            {
                if(IsDead)
                    onDeathChannel?.Invoke(this);
                else
                    onAliveChannel?.Invoke(this);
            }
        
            healthPercentChannel?.Invoke(_currentHealthData.currentHealth / _currentHealthData.maxHealth);

        }

        public bool IsDead => _currentHealthData.currentHealth <= 0;
        public bool IsAlive => !IsDead;
    
        public void TakeDamage(float damage)
        {
            SetCurrentHealth(Math.Max(_currentHealthData.currentHealth - damage, 0f)); 
        }

    }
}
