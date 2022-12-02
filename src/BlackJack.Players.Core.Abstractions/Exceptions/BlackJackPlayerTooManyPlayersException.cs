using BlackJack.Players.Core.Abstractions.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.Exceptions;

public class BlackJackPlayerTooManyPlayersException : BlackJackPlayerException
{
    public BlackJackPlayerTooManyPlayersException( int maxPlayers, Exception? ex = null) 
        : base(BlackJackPlayerErrorCode.TooManyPlayers, $"The maximum amount of players {maxPlayers} is reached for this table", ex)
    {
    }
}