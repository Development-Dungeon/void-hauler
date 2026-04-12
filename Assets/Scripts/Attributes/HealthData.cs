using UnityEngine;

namespace Attributes
{
    [CreateAssetMenu(fileName = "Health", menuName = "Attributes/Health")]
    public class HealthData : ScriptableObject
    {
        public float currentHealth;
        public float maxHealth;
    }
}