using BlackJack.Players.Core.Abstractions.DomainModels;

namespace BlackJack.Players.Core.Abstractions.Repositories;

public interface IBlackJackPlayersRepository
{
    Task<IBlackJackPlayer> Get(Guid id);
    Task<bool> Create(IBlackJackPlayer domainModel);
    Task<bool> Update(Guid id, IBlackJackPlayer domainModel);
}