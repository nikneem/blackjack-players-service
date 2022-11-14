using BlackJack.Players.Core.Abstractions.DataTransferObjects;
using BlackJack.Players.Core.Abstractions.ErrorCodes;
using BlackJack.Players.Core.Abstractions.Exceptions;
using BlackJack.Players.Core.Abstractions.Repositories;
using BlackJack.Players.Core.Abstractions.Services;
using BlackJack.Players.Core.DomainModels;

namespace BlackJack.Players.Core.Services;

public class BlackJackPlayersService: IBlackJackPlayersService
{
    private readonly IBlackJackPlayersRepository _repository;

    public async Task<PlayerDetailsDto> CreateAsync(PlayerCreateDto dto)
    {
        try
        {
            var player = BlackJackPlayer.Create(dto.DisplayName);
            if (await _repository.Create(player))
            {
                return new PlayerDetailsDto
                {
                    Id = player.Id,
                    DisplayName = player.DisplayName,
                };
            }
        }
        catch (Exception ex)
        {
            throw new BlackJackPlayerOperationException(BlackJackPlayerErrorCode.CreationFailure, ex);
        }

        throw new BlackJackPlayerOperationException(BlackJackPlayerErrorCode.CreationFailure);
    }

    public async Task<PlayerDetailsDto> UpdateAsync(Guid id, PlayerDetailsDto dto)
    {
        try
        {
            var player = await _repository.Get(id);
            player.SetDisplayName(dto.DisplayName);
            if (await _repository.Update(id, player))
            {
                return new PlayerDetailsDto
                {
                    Id = id,
                    DisplayName = player.DisplayName,
                };
            }
        }
        catch (Exception ex)
        {
            throw new BlackJackPlayerOperationException(BlackJackPlayerErrorCode.UpdateFailure, ex);
        }

        throw new BlackJackPlayerOperationException(BlackJackPlayerErrorCode.UpdateFailure);
    }

    public BlackJackPlayersService(IBlackJackPlayersRepository repository)
    {
        _repository = repository;
    }
}