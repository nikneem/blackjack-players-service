using BlackJack.Players.Core.Abstractions.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.Exceptions;

public class BlackJackPlayerOperationException : BlackJackPlayerException
{
    public BlackJackPlayerOperationException(BlackJackPlayerErrorCode errorCode, Exception? inner = null)
        : base(errorCode, $"Error code {errorCode.Code} - Player operation failed (could not create/update/delete)", inner)
    {
    }
}