using Microsoft.Extensions.DependencyInjection;
using Sportradar.LiveOddsService.Domain.Services;

namespace Sportradar.LiveOddsService.Business.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMatchServices(this IServiceCollection services) {
            return services.AddScoped<IMatchService, MatchService>();
        }
    }
}
