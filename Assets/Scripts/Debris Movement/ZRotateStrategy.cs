using UnityEngine;

namespace Debris_Movement
{
    [CreateAssetMenu(fileName = "ZRotateStrategy", menuName = "RotationStrategy/ZRotationStrategy")]
    public class ZRotateStrategy : RotateStrategy 
    {
        public float rotationSpeed;
        
        public override void Invoke(Transform transform)
        {
            transform.Rotate(new Vector3(0, 0, 1), Time.deltaTime * rotationSpeed);
        }
    }
}