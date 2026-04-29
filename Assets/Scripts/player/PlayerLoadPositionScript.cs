using System;
using UnityEngine;
using VContainer;

namespace player
{
    public class PlayerLoadPositionScript : MonoBehaviour
    {
        public PositionSo playerPosition;
        
        private PlayerMovementController _playerMovement;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _playerMovement = resolver.Resolve<PlayerMovementController>();
        }

        private void Start()
        {
            _playerMovement.Body.position = playerPosition.PopPositionOrDefault();
        }
    }
}
