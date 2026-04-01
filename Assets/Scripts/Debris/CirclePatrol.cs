using System;
using UnityEngine;

namespace Debris
{
    public class CirclePatrol : MonoBehaviour 
    {
        private float _angle;
        public float speed = 1;
        public float radius = 0.67f;
        private Vector3 _centerPoint;
        public float rotationSpeed = 100f;

        private void Awake()
        {
            _centerPoint = transform.position;
        }

        private void Update()
        {
            // 1. Increment the angle based on time and speed
            _angle += speed * Time.deltaTime;

            // 2. Calculate the new X and Y positions (for a horizontal circle)
            float x = Mathf.Cos(_angle) * radius;
            float y = Mathf.Sin(_angle) * radius;

            // 3. Set the position relative to the center point
            Vector3 offset = new Vector3(x, y, 0);
            transform.position = Vector3.Lerp(transform.position, _centerPoint + offset, Time.deltaTime * speed);
            
            // Rotation (spin on Z axis)
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

    }
}