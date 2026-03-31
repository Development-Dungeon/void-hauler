using TMPro;
using UnityEngine;

namespace Utility
{
    public class FloatToTextAdaptor : MonoBehaviour
    {
        [Get] public TMP_Text text;
    
        [Tooltip("this field accepts float formating")]
        public string format = "0.00";

        public void SetTextFromFloat(float value)
        {
            if (text == null)
                return;
            text.text = value.ToString(format); 
        }

    }
}
