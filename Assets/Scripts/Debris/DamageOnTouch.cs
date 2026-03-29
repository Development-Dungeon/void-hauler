using System;
using Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Debris
{
    public class DamageOnTouch : MonoBehaviour
    {
        public float contactDamage = 10;
        
        private void OnCollisionEnter(Collision other)
        {
            var health = other.gameObject.GetComponent<Health>();
            
            health?.TakeDamage(contactDamage);
        }
    }
}