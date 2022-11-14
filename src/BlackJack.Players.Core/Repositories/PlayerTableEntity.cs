using Azure;
using Azure.Data.Tables;

namespace BlackJack.Players.Core.Repositories;

public class PlayerTableEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string DisplayName { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}