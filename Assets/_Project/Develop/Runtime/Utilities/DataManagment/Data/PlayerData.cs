using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment.Data
{
    public class PlayerData: ISaveData
    {
        public Dictionary<CurrencyTypes, int> WalletData;

        public int WinCount;

        public int LossCount;
    }
}
