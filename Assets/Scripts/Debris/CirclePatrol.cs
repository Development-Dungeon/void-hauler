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
        public bool faceDirection;
        private Vector3 _lastPosition;
        public bool enableScript = true;
       

        private void Awake()
        {
            _centerPoint = transform.position;
        }

        private void Update()
        {
            if (!enableScript) return;
            
            // 1. Increment the angle based on time and speed
            _angle += speed * Time.deltaTime;

            // 2. Calculate the new X and Y positions (for a horizontal circle)
            float x = Mathf.Cos(_angle) * radius;
            float y = Mathf.Sin(_angle) * radius;

            // 3. Set the position relative to the center point
            Vector3 offset = new Vector3(x, y, 0);
            _lastPosition = transform.position;
            transform.position = Vector3.Lerp(transform.position, _centerPoint + offset, Time.deltaTime * speed);

            if (faceDirection)
            {
                Vector3 moveDelta = transform.position - _lastPosition;

                // Ensure we actually moved to avoid NaN errors or flickering
                if (moveDelta.sqrMagnitude > 0.000001f) 
                {
                    float angle = Mathf.Atan2(moveDelta.y, moveDelta.x) * Mathf.Rad2Deg;
            
                    // Adjust the -90 offset depending on your sprite's "Forward"
                    // If it's still slightly off, try -85 or -95 to account for the Lerp lag, 
                    // but -90 is the mathematical target for a "tip-up" triangle.
                    transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                }
            }
            
            
        }

    }
}