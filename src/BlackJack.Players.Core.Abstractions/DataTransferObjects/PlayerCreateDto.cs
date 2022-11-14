using System.ComponentModel.DataAnnotations;

namespace BlackJack.Players.Core.Abstractions.DataTransferObjects;

public class PlayerCreateDto
{
    [MaxLength(20)] [Required] public string DisplayName { get; set; } = null!;
}