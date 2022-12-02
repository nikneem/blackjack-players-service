using BlackJack.Core.Exceptions;
using BlackJack.Players.Core.Abstractions.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.Exceptions;

public class BlackJackPlayerException : BlackJackException
{
    public BlackJackPlayerException(BlackJackPlayerErrorCode errorCode, string message, Exception? ex=null) : base(errorCode, message, ex)
    {
    }
}