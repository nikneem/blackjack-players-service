namespace BlackJack.Players.Core.Abstractions.DomainModels;

public interface IBlackJackPlayer
{
    public string DisplayName { get; }

    void SetDisplayName(string value);
}