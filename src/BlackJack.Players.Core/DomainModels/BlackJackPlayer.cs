using BlackJack.Players.Core.Abstractions.DomainModels;
using BlackJack.Players.Core.Abstractions.Exceptions;
using HexMaster.DomainDrivenDesign;
using HexMaster.DomainDrivenDesign.ChangeTracking;

namespace BlackJack.Players.Core.DomainModels
{
    public class BlackJackPlayer : DomainModel<Guid>, IBlackJackPlayer
    {
        public string DisplayName { get; private set; } = null!;
        public Guid UserId { get; }
        public Guid SessionId { get; }
        public bool IsDealer { get; private set; }
        public int Order { get; set; }

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
        public void SetDealer(bool value)
        {
            if (!Equals(IsDealer, value))
            {
                IsDealer = value;
                SetState(TrackingState.Modified);
            }
        }

        public BlackJackPlayer(Guid id, Guid userId, Guid sessionId,string displayName, bool isDealer, int order) : base(id)
        {
            UserId = userId;
            SessionId = sessionId;
            DisplayName = displayName;
            IsDealer = isDealer;
            Order = order;
        }
        private BlackJackPlayer(Guid userId, Guid sessionId) : base(Guid.NewGuid(), TrackingState.New)
        {
            UserId = userId;
            SessionId = sessionId;
        }

        public static BlackJackPlayer Create(Guid userId, Guid sessionId, string displayName, int order)
        {
            var player = new BlackJackPlayer(userId, sessionId)
                {Order = order};
            player.SetDisplayName(displayName);
            return player;
        }

    }
}
