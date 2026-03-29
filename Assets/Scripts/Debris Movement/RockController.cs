using UnityEngine;

namespace Debris_Movement
{
    public class RockController : MonoBehaviour
    {
    
        public bool canRotate = true;
        public bool canPatrol = true;
    
        public RotateStrategy rotationStrategy;
        public PatrolPattern patrolPattern;

        public float radius = 4;

        void Update()
        {
            if(canRotate)
                rotationStrategy.Invoke(transform);
            if(canPatrol)
                patrolPattern.Invoke(transform, radius);
        }


    }
}