using BlackJack.Players.Core.Abstractions.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.Exceptions;

public class BlackJackPlayerNameInvalidException : BlackJackPlayerException
{
    public BlackJackPlayerNameInvalidException(Exception? inner = null) 
        : base(BlackJackPlayerErrorCode.PlayerNameInvalid, $"Error code {BlackJackPlayerErrorCode.PlayerNameInvalid.Code} - The player name is invalid", inner)
    {
    }
}