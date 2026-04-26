using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeScreenData", menuName = "Shop/Upgrade Screen Data")]
    public class UpgradeScreenData : ScriptableObject 
    {
        public MerchantShopData merchantShopData;
        public Vector3 playerLocationForRespawn;
        public LaunchCost launchCost;

        public void SetValues(MerchantShopData msd, Transform t, LaunchCost lc)
        {
            merchantShopData = msd;
            playerLocationForRespawn = t.position;
            launchCost = lc;
        }
    }
}