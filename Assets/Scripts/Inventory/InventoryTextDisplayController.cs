using TMPro;
using UnityEngine;
using Utility;

namespace Inventory
{
    [RequireComponent(typeof(TMP_Text))]
    public class InventoryTextDisplayController : MonoBehaviour
    {
        public InventoryData inventoryData;
        public InventoryCounter counterStrategy;
        public string outputTextFormat = "0";
        
        [Get] public TMP_Text outputText;

        void Start()
        {
            inventoryData.OnItemAdded += OnItemAdded;
            inventoryData.OnItemRemoved += OnItemRemoved;
            UpdateText();
        }

        private void OnItemRemoved(ItemType arg1, float arg2)
        {
            UpdateText();
        }

        private void OnItemAdded(ItemType arg1, float arg2)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            var counter = counterStrategy.Calculate(inventoryData);

            if (counter == 0)
            {
                outputText.text = "";
            }
            else
            {
                outputText.text = counter.ToString(outputTextFormat);
            }
        }

        private void OnDestroy()
        {
            inventoryData.OnItemAdded -= OnItemAdded;
            inventoryData.OnItemRemoved -= OnItemRemoved;
        }
    }
}