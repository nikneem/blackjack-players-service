using BlackJack.Players.Core.Abstractions.DomainModels;

namespace BlackJack.Players.Core.Abstractions.DataTransferObjects;

public class PlayerDetailsDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string DisplayName { get; set; }
    public int Order { get; set; }
    public bool IsDealer{ get; set; }

    public static PlayerDetailsDto FromDomainModel(Guid id, IBlackJackPlayer player)
    {
        return new PlayerDetailsDto
        {
            Id = id,
            UserId = player.UserId,
            DisplayName = player.DisplayName,
            Order = player.Order,
            IsDealer = player.IsDealer
        };
    }
}