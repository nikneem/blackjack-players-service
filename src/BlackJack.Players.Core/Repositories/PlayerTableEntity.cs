using Azure;
using Azure.Data.Tables;

namespace BlackJack.Players.Core.Repositories;

public class PlayerTableEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public string DisplayName { get; set; }
    public bool IsDealer { get; set; }
    public int Order { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}