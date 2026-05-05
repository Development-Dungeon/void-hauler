using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeUIController : MonoBehaviour
    {
        [Header("UI")] public TMP_Text upgradeName;
        public TMP_Text upgradeDescription;
        public TMP_Text upgradeStatDescription;
        public TMP_Text price;
        public Button button;
        public Color32 canBuyColor = new Color32(255,255,255,255);
        public Color32 cannotBuyColor = new Color32(255,0,0,255);
        public Color32 activeColor = new Color32(127, 255, 0, 255); 
        public Image upgradeIcon;
        
        [Header("Data - Do Not Modify")] public UpgradeEntry upgradeEntry;
        public bool canBuy;
        public bool unavailable;

        public void Init(UpgradeEntry ue)
        {
            upgradeEntry = ue;
        }

        private void RefreshUI()
        {
            if (upgradeName != null) upgradeName.text = GetName();
            if (upgradeDescription != null) upgradeDescription.text = GetDescription();
            if (upgradeStatDescription != null) upgradeStatDescription.text = GetStatDescription();
            if (upgradeIcon != null) upgradeIcon.sprite = upgradeEntry.icon;
            if (price != null)
            {
                if (upgradeEntry.purchased)
                {
                    price.text = "ACTIVE";
                    price.color = activeColor;
                }
                else
                {
                    price.text = GetPrice();
                    price.color = canBuy ? canBuyColor : cannotBuyColor;
                }
            }

            button.interactable = !upgradeEntry.purchased;
            if (unavailable) SetUnavailable();
        }

        private string GetName() => upgradeEntry == null ? "" : upgradeEntry.name;
        private string GetDescription() => upgradeEntry == null ? "" : upgradeEntry.description;
        private string GetStatDescription() => upgradeEntry == null ? "" : upgradeEntry.stateDescription;
        private string GetPrice() => upgradeEntry == null ? "" : upgradeEntry.price.ToString("$0");

        private void SetUnavailable()
        {
            if (button == null) return;

            if (upgradeEntry.purchased) return;

            button.interactable = false;
        }

        public void RefreshUI(bool _canBuy, bool _unavilable)
        {
            canBuy = _canBuy;
            unavailable = _unavilable;
            RefreshUI();
        }
    }
}