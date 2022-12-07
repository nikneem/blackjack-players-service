using BlackJack.Players.Core.Abstractions.DataTransferObjects;

namespace BlackJack.Players.Core.Abstractions.Services;

public interface IBlackJackPlayersService
{
    Task<List<PlayerDetailsDto>> ListAsync(Guid sessionId);
    Task<PlayerDetailsDto> CreateAsync(PlayerCreateDto dto);
    Task<bool> CreateDealerAsync(Guid userId, Guid sessionId);
    Task<PlayerDetailsDto> UpdateAsync(Guid id, PlayerDetailsDto dto);
}