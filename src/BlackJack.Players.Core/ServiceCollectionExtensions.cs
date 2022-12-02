using BlackJack.Players.Core.Abstractions.Repositories;
using BlackJack.Players.Core.Abstractions.Services;
using BlackJack.Players.Core.Repositories;
using BlackJack.Players.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlackJack.Players.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlackJackPlayers(this IServiceCollection services)
    {
        services.AddTransient<IBlackJackPlayersService, BlackJackPlayersService>();
        services.AddTransient<IBlackJackPlayersRepository, BlackJackPlayersRepository>();
        return services;
    }
}