using System;
using Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Debris
{
    public class DamageOnTouch : MonoBehaviour
    {
        public float contactDamage = 10;
        public float damageCooldownTimer = .5f;
        public bool waitingForExit = false;
        private CountdownTimer _damageTimer;
        private Health _health;

        private void Start()
        {
           _damageTimer = new CountdownTimer(damageCooldownTimer);
           _damageTimer.OnTimerStop += OnDamageTimer;
        }

        private void OnDamageTimer()
        {
            // at this point i must still be inside the object so i should take more damage
            if (_health == null)
            {
                // i won't restart the timer. i think this block is never reachable 
                return;
            }
            
            _health.TakeDamage(contactDamage);
            
            StartTimer();
        }

        private void Update()
        {
            if(_damageTimer.IsRunning)
                _damageTimer.Tick(Time.deltaTime);
        }

        private void OnDestroy()
        {
           _damageTimer.OnTimerStop -= OnDamageTimer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (waitingForExit) return;
            
            _health = other.gameObject.GetComponent<Health>();

            if (_health == null) return;
            
            _health.TakeDamage(contactDamage);
            
            waitingForExit = true;

            StartTimer();
        }

        private void OnTriggerExit(Collider other)
        {
            waitingForExit = false;

            _health = null;

            StartTimer();
        }

        private void StartTimer()
        {
            if (!(damageCooldownTimer > 0)) return;
            
            _damageTimer.Reset(damageCooldownTimer);
            _damageTimer.Start();
        }
    }
}