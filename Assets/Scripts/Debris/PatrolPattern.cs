using UnityEngine;

namespace Debris
{
    public abstract class PatrolPattern : ScriptableObject
    {
        public abstract void Invoke(Transform transform, float radius);
    }
}