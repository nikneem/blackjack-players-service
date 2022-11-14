using BlackJack.Players.Core.Abstractions.DataTransferObjects;
using BlackJack.Players.Core.Abstractions.DomainModels;
using BlackJack.Players.Core.Abstractions.Repositories;
using BlackJack.Players.Core.Abstractions.Services;
using BlackJack.Players.Core.DomainModels;
using BlackJack.Players.Core.Services;
using FluentAssertions;
using Moq;

namespace BlackJack.Players.Core.Tests.Services;

public class PlayersServiceTests
{

    private IBlackJackPlayersService _playersService;
    private Mock<IBlackJackPlayersRepository> _repositoryMock;

    [Fact]
    public async Task WhenPlayerCreated_ThePlayerIsPersisted()
    {
        WhenPlayerCreationSucceeds();
        var desiredDisplayName = "Henk";
        var act = await _playersService.CreateAsync(new PlayerCreateDto
        {
            DisplayName = desiredDisplayName
        });
        act.DisplayName.Should().Be(desiredDisplayName);
        _repositoryMock.Verify(repo => repo.Create(It.IsAny<IBlackJackPlayer>()), Times.Once);
    }

    [Fact]
    public async Task WhenPlayerUpdated_ThePlayerIsPersisted()
    {
        var playerId = WithPlayerInRepository();
        WhenPlayerUpdateSucceeds();
        var desiredDisplayName = "Henk";
        var act = await _playersService.UpdateAsync(playerId, new PlayerDetailsDto()
        {
            Id = playerId,
            DisplayName = desiredDisplayName
        });
        act.DisplayName.Should().Be(desiredDisplayName);
        _repositoryMock.Verify(repo => repo.Update(playerId, It.IsAny<IBlackJackPlayer>()), Times.Once);
    }

    private void WhenPlayerCreationSucceeds()
    {
        _repositoryMock.Setup(x => x.Create(It.IsAny<IBlackJackPlayer>())).ReturnsAsync(true);
    }
    private void WhenPlayerUpdateSucceeds()
    {
        _repositoryMock.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<IBlackJackPlayer>())).ReturnsAsync(true);
    }
    private Guid WithPlayerInRepository()
    {
        var playerId = Guid.NewGuid();
        var player = new BlackJackPlayer(playerId, "Any name");
        _repositoryMock.Setup(x => x.Get(playerId)).ReturnsAsync(player);
        return playerId;
    }

    public PlayersServiceTests()
    {
        _repositoryMock = new Mock<IBlackJackPlayersRepository>();
        _playersService = new BlackJackPlayersService(_repositoryMock.Object);
    }

}