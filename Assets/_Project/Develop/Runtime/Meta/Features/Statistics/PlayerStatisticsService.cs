using Assets._Project.Develop.Runtime.Utilities.DataManagment.Data;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProvider;

namespace Assets._Project.Develop.Runtime.Meta.Features.Statistics
{
    public class PlayerStatisticsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private int _winCount;
        private int _lossCount;

        public PlayerStatisticsService(PlayerDataProvider playerDataProvider)
        {
            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);
        }

        public int GetWinCount() => _winCount;

        public int GetLossCount() => _lossCount;

        public void AddWin() => _winCount++;

        public void AddLoss() => _lossCount++;

        public void ReadFrom(PlayerData data)
        {
            _winCount = data.WinCount;
            _lossCount = data.LossCount;
        }

        public void WriteTo(PlayerData data)
        {
            data.WinCount = _winCount;
            data.LossCount = _lossCount;
        }

        public void Reset()
        {
            _winCount = 0;
            _lossCount = 0;
        }
    }
}
