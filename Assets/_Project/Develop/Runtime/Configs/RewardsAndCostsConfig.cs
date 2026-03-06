using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs
{
    [CreateAssetMenu(fileName = "Configs", menuName = "Configs/RewardsAndCostsConfig")]
    public class RewardsAndCostsConfig : ScriptableObject
    {
        public int WinReward;
        public int DefeatCost;
        public int ResetCost;

        public CurrencyTypes Currency = CurrencyTypes.Gold;
    }
}
