using System;
using EventChannel.templates;
using UnityEngine;
using VContainer;

namespace Attributes
{
    public class Health : MonoBehaviour
    {
        // [SerializeField] public float maxHealth = 100f;
        // [SerializeField] private float currentHealth;
        public HealthData templateHealthData;
        private HealthData _currentHealthData;
    
        public EventChannel<float> healthPercentChannel;
        public EventChannel<Health> onDeathChannel;
        public EventChannel<Health> onAliveChannel;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _currentHealthData = resolver.ResolveOrDefault<HealthData>();
        }

        private void Awake()
        {
            if(_currentHealthData == null)
                _currentHealthData = Instantiate(templateHealthData); // this will handle non-players who have health
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
