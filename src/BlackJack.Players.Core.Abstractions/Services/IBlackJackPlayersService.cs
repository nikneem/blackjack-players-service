using BlackJack.Players.Core.Abstractions.DataTransferObjects;

namespace BlackJack.Players.Core.Abstractions.Services;

public interface IBlackJackPlayersService
{
    Task<PlayerDetailsDto> CreateAsync(PlayerCreateDto dto);
    Task<PlayerDetailsDto> UpdateAsync(Guid id, PlayerDetailsDto dto);
}