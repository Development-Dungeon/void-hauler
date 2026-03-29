using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace player
{
    [RequireComponent(typeof(CharacterController))]
    public class Movement : MonoBehaviour
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
        
            var move = new Vector3(_userInput.x,0, _userInput.y);
        
            move = transform.TransformDirection(move);
        
            _controller.Move(move * (moveSpeed * Time.deltaTime));
        }
    }
}
