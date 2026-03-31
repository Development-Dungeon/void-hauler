using System;
using EventChannel.templates;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] public float maxHealth = 100f;
        [SerializeField] private float currentHealth;
    
        public EventChannel<float> healthPercentChannel;
        public EventChannel<Health> onDeathChannel;
        public EventChannel<Health> onAliveChannel;

        private void Awake()
        {
            SetCurrentHealth(maxHealth);
        }

        private void SetCurrentHealth(float toBeCurrentHealth)
        {
            var previousState = IsDead;
        
            currentHealth = toBeCurrentHealth;

            var newState = IsDead;

            if (previousState != newState) 
            {
                if(IsDead)
                    onDeathChannel?.Invoke(this);
                else
                    onAliveChannel?.Invoke(this);
            }
        
            healthPercentChannel?.Invoke(currentHealth / maxHealth);

        }

        public bool IsDead => currentHealth <= 0;
        public bool IsAlive => !IsDead;
    
        public void TakeDamage(float damage)
        {
            SetCurrentHealth(Math.Max(currentHealth - damage, 0f)); 
        }

    }
}
