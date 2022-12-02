using BlackJack.Players.Core.Abstractions.DataTransferObjects;
using BlackJack.Players.Core.Abstractions.DomainModels;

namespace BlackJack.Players.Core.Abstractions.Repositories;

public interface IBlackJackPlayersRepository
{
    Task<List<PlayerDetailsDto>> ListAsync(Guid sessionId);
    Task<IBlackJackPlayer> GetAsync(Guid id);
    Task<bool> CreateAsync(IBlackJackPlayer domainModel);
    Task<bool> UpdateAsync(Guid id, IBlackJackPlayer domainModel);

    Task<int> CountPlayersAsync(Guid sessionId);
    Task<bool> GetHasDealerAsync(Guid sessionId);
    Task<bool> GetExistsAsync(Guid userId, Guid sessionId);
}