using System;

namespace Sportradar.LiveOddsService.Domain.Models {
    public class Match {
        public string HomeTeam { get; set; } = default!;
        public string AwayTeam { get; set; } = default!;
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
    }
}
