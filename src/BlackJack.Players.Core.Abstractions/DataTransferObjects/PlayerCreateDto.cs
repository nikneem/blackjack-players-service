using System.ComponentModel.DataAnnotations;

namespace BlackJack.Players.Core.Abstractions.DataTransferObjects;

public class PlayerCreateDto
{
    public Guid UserId { get; set; } 
    public Guid SessionId { get; set; }
    [MaxLength(20)][Required] public string DisplayName { get; set; } = null!;
    public bool IsDealer { get; set; }
}