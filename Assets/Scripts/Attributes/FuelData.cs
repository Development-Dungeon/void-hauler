using UnityEngine;

namespace Attributes
{
    [CreateAssetMenu(fileName = "Fuel", menuName = "Attributes/Fuel")]
    public class FuelData: ScriptableObject
    {
        public float maxFuel;
        public float fuelPerMeter;
        public float currentFuel;
    }
}