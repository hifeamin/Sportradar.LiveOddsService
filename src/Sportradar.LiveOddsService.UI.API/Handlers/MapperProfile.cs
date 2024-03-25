using AutoMapper;
using Sportradar.LiveOddsService.Domain.Models;
using Sportradar.LiveOddsService.UI.API.Models;

namespace Sportradar.LiveOddsService.UI.API.Handlers {
    public class MapperProfile: Profile {
        public MapperProfile() {
            CreateMap<UpdateMatchModel, Match>();
        }
    }
}
