using System;
using player;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace Upgrades
{
    public class MerchantController : MonoBehaviour
    {
        
        
        public MerchantShopData merchantShopData;
        public UpgradeScreenData upgradeScreenData;
        public UnityEvent OnTouch;
        public PositionSo playerPosition;
        public Transform respawnPoint;

        public float onAwakeDelayTriggerBySeconds = 1f;
        private CountdownTimer _countdownTimer;

        private void Awake()
        {
            _countdownTimer = new CountdownTimer(onAwakeDelayTriggerBySeconds);
        }

        private void Start()
        {
            _countdownTimer.Start();
        }

        private void Update()
        {
            if(_countdownTimer.IsRunning)
                _countdownTimer.Tick(Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            if (_countdownTimer.IsRunning)
                return;
            
            upgradeScreenData.SetValues(merchantShopData, other.transform, merchantShopData.launchCost);
            playerPosition.Position = respawnPoint.position;
            OnTouch?.Invoke();
        }

    }
}
