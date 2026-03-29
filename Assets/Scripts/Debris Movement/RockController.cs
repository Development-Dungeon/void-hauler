using UnityEngine;

namespace Debris_Movement
{
    public class RockController : MonoBehaviour
    {
        // rotate
        // i would love to have several different movements pattersn
        // rotate
        // move in a circle
        // move in a line
        // move randomly between points
        // the collision also has to have some type of interaction
        // the interactions can be either pick up
        // the intereaction can be do some damage
        // i would love to have these be different plug and play things
    
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