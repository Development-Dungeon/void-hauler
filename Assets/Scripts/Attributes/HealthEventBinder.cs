using System;
using EventChannel.concrete;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace Attributes
{
    public class HealthEventBinder : MonoBehaviour
    {
        
        [Get] public Health health;
        public FloatEventListener onHealthPercentageListener;
        public HealthEventListener onDeathListener;
        public HealthEventListener onAliveListener;
        
        private void Start()
        {
            if (health == null) return;

            if (onHealthPercentageListener != null)
                onHealthPercentageListener.SetChannel(health.healthPercentChannel);
            if (onDeathListener != null)
                onDeathListener.SetChannel(health.onDeathChannel);
            if(onAliveListener != null)
                onAliveListener.SetChannel(health.onAliveChannel);
        }
    }
}