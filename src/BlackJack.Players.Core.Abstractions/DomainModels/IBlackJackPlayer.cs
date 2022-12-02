namespace BlackJack.Players.Core.Abstractions.DomainModels;

public interface IBlackJackPlayer
{
    public string DisplayName { get; }
    public Guid UserId { get; }
    public Guid SessionId { get; }
    public bool IsDealer { get; }
    public int Order { get; set; }


    void SetDisplayName(string value);
    void SetDealer(bool value);
}