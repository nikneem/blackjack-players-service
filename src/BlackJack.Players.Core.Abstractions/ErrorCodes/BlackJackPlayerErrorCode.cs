using BlackJack.Core.ErrorCodes;

namespace BlackJack.Players.Core.Abstractions.ErrorCodes;

public abstract class BlackJackPlayerErrorCode : BlackJackErrorCode
{

    public static BlackJackPlayerErrorCode PlayerNameInvalid = new BlackJackPlayerNameInvalidErrorCode();
    public static BlackJackPlayerErrorCode CreationFailure = new BlackJackPlayerCreationFailureErrorCode();
    public static BlackJackPlayerErrorCode UpdateFailure = new  BlackJackPlayerUpdateFailureErrorCode();

    public virtual string TranslationErrorCode => $"BlackJackPlayer.{Code}";


}