using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Inject]
        private CharacterController _controller;
        public float moveSpeed = 2.0f;
        private Vector2 _userInput;

        public void OnMove(InputValue value)
        {
            _userInput = value.Get<Vector2>();
        }

        void Update()
        {
            if (_userInput == Vector2.zero)
                return;
        
            var move = new Vector3(_userInput.x,_userInput.y, 0);
        
            move = transform.TransformDirection(move);
        
            _controller.Move(move * (moveSpeed * Time.deltaTime));
        }
    }
}
