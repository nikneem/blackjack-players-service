using BlackJack.Core.Exceptions;
using BlackJack.Players.Core.Abstractions.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.Exceptions;

public class BlackJackPlayerException : BlackJackException
{
    protected BlackJackPlayerException(BlackJackPlayerErrorCode errorCode, string message, Exception? ex) : base(errorCode, message, ex)
    {
    }
}