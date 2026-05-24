using System;
using System.Collections.Generic;
using Attributes;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Debris
{
    public class DamageOnTouchController
    {
        
        private readonly float _contactDamage ;
        private readonly float _damageCooldownTimer ;
        private readonly CountdownTimer _damageTimer;
        private readonly Health _currentOtherHealth;

        public DamageOnTouchController(float contactDamage, float damageCooldownTimer, Health currentOtherHealth)
        {
            _contactDamage = contactDamage;
            _damageCooldownTimer = damageCooldownTimer;
            _currentOtherHealth = currentOtherHealth;
            
            // set the timer
            _damageTimer = new CountdownTimer(damageCooldownTimer);
            _damageTimer.OnTimerStop += OnDamageTimer;
            _damageTimer.Start();
        }

        private void OnDamageTimer()
        {
            // at this point i must still be inside the object so i should take more damage
            if (_currentOtherHealth == null)
            {
                // i won't restart the timer. i think this block is never reachable 
                return;
            }
            
            _currentOtherHealth.TakeDamage(_contactDamage);
            
            StartTimer();
        }
        
        private void StartTimer()
        {
            if (!(_damageCooldownTimer > 0)) return;
            
            _damageTimer.Reset(_damageCooldownTimer);
            _damageTimer.Start();
        }

        public void Tick(float deltaTime)
        {
            if (_damageTimer == null) return;
            
            if(_damageTimer.IsRunning) 
                _damageTimer.Tick(deltaTime);
        }

        public void TakeDamage()
        {
           OnDamageTimer(); 
        }
    }
    public class DamageOnTouch : MonoBehaviour
    {
        public float contactDamage = 10;
        public float damageCooldownTimer = .5f;
        public bool destroyOnTouch = false;
        private Collider2D _ignoreCollider;

        public void Init(float damage, bool destroySelfOnTouch, Collider2D otherCollider = null)
        {
            contactDamage = damage;
            destroyOnTouch= destroySelfOnTouch;
            _ignoreCollider = otherCollider;
        }


        private Dictionary<Collider2D, DamageOnTouchController> _touches = new();

        private void Update()
        {
            foreach (var keyValuePair in _touches)
            {
                var touch = keyValuePair.Value;
                touch.Tick(Time.deltaTime);
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == _ignoreCollider) return; 
            
            var health = other.gameObject.GetComponent<Health>();

            if (health == null) return;

            var damageController = new DamageOnTouchController(contactDamage, damageCooldownTimer, health);

            damageController.TakeDamage();
            
            if(destroyOnTouch)
                Destroy(this.gameObject);
            else 
                _touches.Add(other,damageController) ;
            
        }

        private void OnDestroy()
        {
            _touches.Clear();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == _ignoreCollider) return; 
            _touches.Remove(other);
        }

    }
}