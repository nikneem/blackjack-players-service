using BlackJack.Players.Core.Abstractions;
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

    public Task<List<PlayerDetailsDto>> ListAsync(Guid sessionId)
    {
        return _repository.ListAsync(sessionId);
    }

    public async Task<PlayerDetailsDto> CreateAsync(PlayerCreateDto dto)
    {
        try
        {
            var userAlreadyExists = await _repository.GetExistsAsync(dto.UserId, dto.SessionId);
            var sessionHasDealer = await _repository.GetHasDealerAsync(dto.SessionId);
            var currentActivePlayers = await _repository.CountPlayersAsync(dto.SessionId);

            if (currentActivePlayers > Constants.DefaultMaximumPlayers)
            {
                throw new BlackJackPlayerTooManyPlayersException(Constants.DefaultMaximumPlayers);
            }

            if (dto.IsDealer && sessionHasDealer)
            {
                throw new BlackJackPlayerException(
                    BlackJackPlayerErrorCode.SessionAlreadyHasDealer,
                    "This session already has a dealer, cannot add a new player as dealer");
            }

            if (userAlreadyExists)
            {
                throw new BlackJackPlayerException(
                    BlackJackPlayerErrorCode.UserAlreadyIsPlayer,
                    $"User {dto.UserId} is already registered as player of this table");
            }

            var player = BlackJackPlayer.Create(
                dto.UserId,
                dto.SessionId,
                dto.DisplayName,
                ++currentActivePlayers);

            player.SetDealer(dto.IsDealer);

            if (await _repository.CreateAsync(player))
            {
                return PlayerDetailsDto.FromDomainModel(player.Id, player);
            }
        }
        catch (Exception ex)
        {
            throw new BlackJackPlayerOperationException(BlackJackPlayerErrorCode.CreationFailure, ex);
        }

        throw new BlackJackPlayerOperationException(BlackJackPlayerErrorCode.CreationFailure);
    }

    public async Task<bool> CreateDealerAsync(Guid userId, Guid sessionId)
    {
        var alreadyHasDealer = await _repository.GetHasDealerAsync(sessionId);
        if (alreadyHasDealer)
        {
            throw new BlackJackPlayerException(
                BlackJackPlayerErrorCode.SessionAlreadyHasDealer,
                $"The session {sessionId} already has a dealer player");
        }

        var player = BlackJackPlayer.Create(userId, sessionId, "Dealer", 0);
        player.SetDealer(true);
        return await _repository.CreateAsync(player);
    }

    public async Task<PlayerDetailsDto> UpdateAsync(Guid id, PlayerDetailsDto dto)
    {
        try
        {
            var player = await _repository.GetAsync(id);
            player.SetDisplayName(dto.DisplayName);
            if (await _repository.UpdateAsync(id, player))
            {
                return PlayerDetailsDto.FromDomainModel(id, player);
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