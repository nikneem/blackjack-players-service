using BlackJack.Players.Core.Abstractions.DomainModels;
using BlackJack.Players.Core.Abstractions.ErrorCodes;
using BlackJack.Players.Core.Abstractions.Exceptions;
using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;

namespace BlackJack.Players.Core.DomainModels
{
    public class BlackJackPlayer : DomainModel<Guid>, IBlackJackPlayer
    {
        public string DisplayName { get; private set; } = null!;

        public void SetDisplayName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new BlackJackPlayerNameInvalidException();
            }

            if (!Equals(DisplayName, value))
            {
                DisplayName = value;
                SetState(TrackingState.Modified);
            }
        }

        public BlackJackPlayer(Guid id, string displayName) : base(id)
        {
            DisplayName = displayName;
        }
        private BlackJackPlayer() : base(Guid.NewGuid(), TrackingState.New)
        {
        }

        public static BlackJackPlayer Create(string displayName)
        {
            var player = new BlackJackPlayer();
            player.SetDisplayName(displayName);
            return player;
        }
    }
}
