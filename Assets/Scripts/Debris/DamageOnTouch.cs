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


        private Dictionary<Collider, DamageOnTouchController> _touches = new();

        private void Update()
        {
            foreach (var keyValuePair in _touches)
            {
                var touch = keyValuePair.Value;
                touch.Tick(Time.deltaTime);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            
            var health = other.gameObject.GetComponent<Health>();

            if (health == null) return;

            var damageController = new DamageOnTouchController(contactDamage, damageCooldownTimer, health);

            damageController.TakeDamage();
                
            _touches.Add(other,damageController) ;
            
        }

        private void OnDestroy()
        {
            _touches.Clear();
        }

        private void OnTriggerExit(Collider other)
        {
            _touches.Remove(other);
        }

    }
}