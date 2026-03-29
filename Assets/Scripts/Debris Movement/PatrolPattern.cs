using UnityEngine;

namespace Debris_Movement
{
    public abstract class PatrolPattern : ScriptableObject
    {
        public abstract void Invoke(Transform transform, float radius);
    }
}