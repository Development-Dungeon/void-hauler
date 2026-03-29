using UnityEngine;

namespace Debris_Movement
{
    [CreateAssetMenu(fileName = "PatrolPattern", menuName = "PatrolPattern/CirclePatrolStrategy")]
    public class CirclePatrolStrategy : PatrolPattern
    {
        private float angle;
        public float speed;
        private Transform centerPoint; 
    
        public override void Invoke(Transform transform, float radius)
        {
            if (centerPoint == null)
                centerPoint = transform;
        
            // 1. Increment the angle based on time and speed
            angle += speed * Time.deltaTime;

            // 2. Calculate the new X and Y positions (for a horizontal circle)
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // 3. Set the position relative to the center point
            Vector3 offset = new Vector3(x, y, 0);
            transform.position = Vector3.Lerp(transform.position, centerPoint.position + offset, Time.deltaTime * speed);
        }
    }
}