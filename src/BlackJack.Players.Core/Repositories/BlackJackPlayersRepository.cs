using Azure;
using Azure.Data.Tables;
using BlackJack.Core.Factories;
using BlackJack.Players.Core.Abstractions.DomainModels;
using BlackJack.Players.Core.Abstractions.Repositories;
using BlackJack.Players.Core.DomainModels;

namespace BlackJack.Players.Core.Repositories;

public class BlackJackPlayersRepository : IBlackJackPlayersRepository
{
    private readonly IStorageTableClientFactory _tableStorageClientFactory;

    private const string TableName = "players";
    private const string PartitionKey = "player";

    public async Task<IBlackJackPlayer> Get(Guid id)
    {
        var tableClient = _tableStorageClientFactory.CreateClient(TableName);
        var entity = await tableClient.GetEntityAsync<PlayerTableEntity>(PartitionKey, id.ToString());
        return new BlackJackPlayer(
            Guid.Parse(entity.Value.RowKey),
            entity.Value.DisplayName);
    }

    public async Task<bool> Create(IBlackJackPlayer domainModel)
    {
        if (domainModel is BlackJackPlayer blackJackPlayer)
        {
            var entity = new PlayerTableEntity
            {
                PartitionKey = PartitionKey,
                RowKey = blackJackPlayer.Id.ToString(),
                DisplayName = blackJackPlayer.DisplayName,
                ETag = ETag.All,
                Timestamp = DateTimeOffset.UtcNow
            };
            var tableClient = _tableStorageClientFactory.CreateClient(TableName);
            var response = await tableClient.AddEntityAsync(entity);
            return !response.IsError;
        }

        throw new Exception("Black Jack player to create is of an unknown or invalid type");
    }

    public async Task<bool> Update(Guid id, IBlackJackPlayer domainModel)
    {
        if (domainModel is BlackJackPlayer blackJackPlayer)
        {
            var entity = new PlayerTableEntity
            {
                PartitionKey = PartitionKey,
                RowKey = blackJackPlayer.Id.ToString(),
                DisplayName = blackJackPlayer.DisplayName,
                ETag = ETag.All,
                Timestamp = DateTimeOffset.UtcNow
            };
            var tableClient = _tableStorageClientFactory.CreateClient(TableName);
            var response = await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
            return !response.IsError;
        }

        throw new Exception("Black Jack player to update is of an unknown or invalid type");
    }

    public BlackJackPlayersRepository(IStorageTableClientFactory tableStorageClientFactory)
    {
        _tableStorageClientFactory = tableStorageClientFactory;
    }

}