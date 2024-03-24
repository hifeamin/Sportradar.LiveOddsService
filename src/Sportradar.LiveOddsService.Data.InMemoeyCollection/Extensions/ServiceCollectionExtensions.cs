using Microsoft.Extensions.DependencyInjection;

namespace Sportradar.LiveOddsService.Data.InMemoeyCollection.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMatchRepositories(this IServiceCollection services) {
            return services.AddSingleton<DbContext>()
                .AddScoped<IMatchRepository, MatchRepository>();
        }
    }
}
