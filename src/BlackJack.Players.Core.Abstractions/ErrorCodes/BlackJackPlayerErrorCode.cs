using BlackJack.Core.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.ErrorCodes;

public abstract class BlackJackPlayerErrorCode : BlackJackErrorCode
{

    public static readonly BlackJackPlayerErrorCode PlayerNameInvalid = new BlackJackPlayerNameInvalidErrorCode();
    public static readonly BlackJackPlayerErrorCode CreationFailure = new BlackJackPlayerCreationFailureErrorCode();
    public static readonly BlackJackPlayerErrorCode UpdateFailure = new BlackJackPlayerUpdateFailureErrorCode();
    public static readonly BlackJackPlayerErrorCode TooManyPlayers = new BlackJackPlayersTooManyPlayersErrorCode();
    public static readonly BlackJackPlayerErrorCode SessionAlreadyHasDealer = new BlackJackPlayerSessionAlreadyHasDealerErrorCode();
    public static readonly BlackJackPlayerErrorCode UserAlreadyIsPlayer = new BlackJackPlayerUserAlreadyIsPlayerErrorCode();

    public override string ErrorNamespace => "Players";


}