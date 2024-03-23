using System.Threading.Tasks;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Domain.Services {
    public interface IMatchService {
        Task<Match> StartAsync(string homeTeam, string awayTeam);
    }
}
