using System;
using UnityEngine;

namespace Debris
{
    public class ZRotator : MonoBehaviour 
    {
        public float rotationSpeed = 5;

        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, 1), Time.deltaTime * rotationSpeed);
        }
    }
}