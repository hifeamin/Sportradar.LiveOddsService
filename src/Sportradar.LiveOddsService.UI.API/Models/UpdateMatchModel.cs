namespace Sportradar.LiveOddsService.UI.API.Models {
    public class UpdateMatchModel {
        public string HomeTeam { get; set; } = default!;
        public string AwayTeam { get; set; } = default!;
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
    }
}
