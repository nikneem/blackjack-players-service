using BlackJack.Players.Core.Abstractions.Exceptions;
using BlackJack.Players.Core.DomainModels;
using FluentAssertions;

namespace BlackJack.Players.Core.Tests.DomainModels;

public class PlayersTests
{
    [Fact]
    public void WhenPlayerNameIsValid_ItSucceeds()
    {
        var oldName = "OldName";
        var newName = "NewName";
        var player = BlackJackPlayer.Create(Guid.NewGuid(), Guid.NewGuid(), oldName, 1);
        player.DisplayName.Should().Be(oldName);
        player.SetDisplayName(newName);
        player.DisplayName.Should().Be(newName);
    }

    [Fact]
    public void WhenPlayerNameIsInvalid_ItMustThrow_BlackJackPlayerNameInvalidException()
    {
        var act = () => { BlackJackPlayer.Create(Guid.NewGuid(), Guid.NewGuid(), string.Empty, 1); };
        act.Should().Throw<BlackJackPlayerNameInvalidException>();
    }

    [Fact]
    public void WhenPlayerNameChangedToInvalid_ItMustThrow_BlackJackPlayerNameInvalidException()
    {
        var oldName = "OldName";
        var player = BlackJackPlayer.Create(Guid.NewGuid(), Guid.NewGuid(), oldName, 1);
        var act = () => { player.SetDisplayName(string.Empty); };
        act.Should().Throw<BlackJackPlayerNameInvalidException>();
    }
}