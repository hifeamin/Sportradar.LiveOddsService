using System.Collections.Generic;
using Sportradar.LiveOddsService.Domain.Models;

namespace Sportradar.LiveOddsService.Data.InMemoeyCollection {
    public class DbContext {
        public IDictionary<string, Match> Matches { get; private set; } = new Dictionary<string, Match>();
    }
}
