using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using VContainer;

public class PlayerGunController : MonoBehaviour
{
    public BulletController bulletPrefab;
    [Tooltip("in seconds")]
    [Min(0)]
    public float fireRate = 0.5f;
    private CountdownTimer _fireTimer;
    public InputAction inputAction;
    [Inject]
    public Camera mainCamera;
    [Get] 
    public Collider2D selfCollider;

    private void OnEnable()
    {
        inputAction.Enable();

        inputAction.performed += StartTimer;
        inputAction.canceled -= StartTimer;

        _fireTimer = new CountdownTimer(fireRate);
        _fireTimer.OnTimerStart += FireGun;
    }


    private void StartTimer(InputAction.CallbackContext obj)
    {
        if (_fireTimer.IsRunning) return;
        
        _fireTimer.Reset(fireRate);
        _fireTimer.Start();
        
    }

    private void Update()
    {
        _fireTimer.Tick(Time.deltaTime);
    }

    private void OnDisable()
    {
        inputAction.Disable();
        _fireTimer.OnTimerStart -= FireGun;
    }

    private void FireGun()
    {
        var mouseScreenPos = Mouse.current.position.ReadValue();
        var targetLocation = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        
        targetLocation.z = transform.position.z; // since we are working in a 2d game
        
        Instantiate(bulletPrefab, transform.position, Quaternion.identity)
            .Init(targetLocation, selfCollider);
        
    }
}
