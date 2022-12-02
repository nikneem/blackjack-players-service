using Azure;
using Azure.Data.Tables;
using BlackJack.Core.Factories;
using BlackJack.Players.Core.Abstractions.DataTransferObjects;
using BlackJack.Players.Core.Abstractions.DomainModels;
using BlackJack.Players.Core.Abstractions.Repositories;
using BlackJack.Players.Core.DomainModels;

namespace BlackJack.Players.Core.Repositories;

public class BlackJackPlayersRepository : IBlackJackPlayersRepository
{
    private readonly IStorageTableClientFactory _tableStorageClientFactory;

    private const string TableName = "players";
    private const string PartitionKey = "player";

    public async Task<List<PlayerDetailsDto>> ListAsync(Guid sessionId)
    {
        var playerDetailsList = new List<PlayerDetailsDto>();
        var tableClient = _tableStorageClientFactory.CreateClient(TableName);
        var tableQuery = tableClient.QueryAsync<PlayerTableEntity>(
            $"{nameof(PlayerTableEntity.PartitionKey)} eq '{PartitionKey}'  && {nameof(PlayerTableEntity.SessionId)} eq '{sessionId}'");

        await foreach (var page in tableQuery.AsPages())
        {
            playerDetailsList.AddRange(page.Values.Select(p => new PlayerDetailsDto
            {
                Id = Guid.Parse(p.RowKey),
                UserId = p.UserId,
                DisplayName = p.RowKey,
                Order = p.Order,
                IsDealer = p.IsDealer
            }));
        }

        return playerDetailsList;
    }

    public async Task<IBlackJackPlayer> GetAsync(Guid id)
    {
        var tableClient = _tableStorageClientFactory.CreateClient(TableName);
        var entity = await tableClient.GetEntityAsync<PlayerTableEntity>(PartitionKey, id.ToString());
        return new BlackJackPlayer(
            Guid.Parse(entity.Value.RowKey),
            entity.Value.UserId,
            entity.Value.SessionId,
            entity.Value.DisplayName,
            entity.Value.IsDealer,
            entity.Value.Order);
    }

    public async Task<bool> CreateAsync(IBlackJackPlayer domainModel)
    {
        if (domainModel is BlackJackPlayer blackJackPlayer)
        {
            var entity = ToEntity(blackJackPlayer);
            var tableClient = _tableStorageClientFactory.CreateClient(TableName);
            var response = await tableClient.AddEntityAsync(entity);
            return !response.IsError;
        }

        throw new Exception("Black Jack player to create is of an unknown or invalid type");
    }

    public async Task<bool> UpdateAsync(Guid id, IBlackJackPlayer domainModel)
    {
        if (domainModel is BlackJackPlayer blackJackPlayer)
        {
            var entity = ToEntity(blackJackPlayer);
            var tableClient = _tableStorageClientFactory.CreateClient(TableName);
            var response = await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
            return !response.IsError;
        }

        throw new Exception("Black Jack player to update is of an unknown or invalid type");
    }

    public async Task<int> CountPlayersAsync(Guid sessionId)
    {
        var tableClient = _tableStorageClientFactory.CreateClient(TableName);
        var tableQuery = tableClient.QueryAsync<PlayerTableEntity>(
            $"{nameof(PlayerTableEntity.PartitionKey)} eq '{PartitionKey}'  && {nameof(PlayerTableEntity.SessionId)} eq '{sessionId}' && {nameof(PlayerTableEntity.IsDealer)} eq false");

        var totalCount = 0;
        await foreach (var page in tableQuery.AsPages())
        {
            totalCount += page.Values.Count;
        }

        return totalCount;
    }

    public async Task<bool> GetHasDealerAsync(Guid sessionId)
    {
        var tableClient = _tableStorageClientFactory.CreateClient(TableName);
        var tableQuery = tableClient.QueryAsync<PlayerTableEntity>(
            $"{nameof(PlayerTableEntity.PartitionKey)} eq '{PartitionKey}'  && {nameof(PlayerTableEntity.SessionId)} eq '{sessionId}' && {nameof(PlayerTableEntity.IsDealer)} eq true");

        var totalCount = 0;
        await foreach (var page in tableQuery.AsPages())
        {
            totalCount += page.Values.Count;
        }

        return totalCount > 0;
    }

    public async Task<bool> GetExistsAsync(Guid userId, Guid sessionId)
    {
        var tableClient = _tableStorageClientFactory.CreateClient(TableName);
        var tableQuery = tableClient.QueryAsync<PlayerTableEntity>(
            $"{nameof(PlayerTableEntity.PartitionKey)} eq '{PartitionKey}'  && {nameof(PlayerTableEntity.SessionId)} eq '{sessionId}' && {nameof(PlayerTableEntity.UserId)} eq '{userId}'");

        var totalCount = 0;
        await foreach (var page in tableQuery.AsPages())
        {
            totalCount += page.Values.Count;
        }

        return totalCount > 0;
    }

    private static PlayerTableEntity ToEntity(BlackJackPlayer player)
    {
        return new PlayerTableEntity
        {
            PartitionKey = PartitionKey,
            RowKey = player.Id.ToString(),
            DisplayName = player.DisplayName,
            SessionId = player.SessionId,
            IsDealer = player.IsDealer,
            UserId = player.UserId,
            Order = player.Order,
            ETag = ETag.All,
            Timestamp = DateTimeOffset.UtcNow
        };
    }

    public BlackJackPlayersRepository(IStorageTableClientFactory tableStorageClientFactory)
    {
        _tableStorageClientFactory = tableStorageClientFactory;
    }

}