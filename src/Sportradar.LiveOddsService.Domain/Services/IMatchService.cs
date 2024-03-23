using System.Threading.Tasks;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Domain.Services {
    public interface IMatchService {
        Task FinishAsync(string homeTeam, string awayTeam);
        Task<Match> StartAsync(string homeTeam, string awayTeam);
        Task UpdateAsync(Match match);
    }
}
