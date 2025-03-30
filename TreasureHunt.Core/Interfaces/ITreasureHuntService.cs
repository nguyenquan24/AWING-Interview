using TreasureHunt.Core.Entities;
using TreasureHunt.Core.Models;

namespace TreasureHunt.Core.Interfaces
{
    public interface ITreasureHuntService
    {
        Task<double> SolveTreasureHuntAsync(TreasureMapRequest request);

        Task<IEnumerable<TreasureMap>> GetHistoryAsync();
    }
}