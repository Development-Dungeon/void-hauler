using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Inject]
    private CharacterController _controller;
    public float moveSpeed = 2.0f;
    private Vector2 _userMovementInput;
    

    public void OnMove(InputValue value)
    {
        _userMovementInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_userMovementInput == Vector2.zero)
            return;
        
        var move = new Vector2(_userMovementInput.x, _userMovementInput.y);
        
        move = transform.TransformDirection(move);
        
        _controller.Move(move * (moveSpeed * Time.deltaTime));
    }
}
