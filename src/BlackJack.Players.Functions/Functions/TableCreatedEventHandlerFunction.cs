using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using BlackJack.Events.Events;
using BlackJack.Players.Core.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlackJack.Players.Functions.Functions;

public class TableCreatedEventHandlerFunction
{
    private readonly ILogger _logger;
    private readonly TableClient _tableClient = null!;

    public TableCreatedEventHandlerFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TableCreatedEventHandlerFunction>();

        var storageAccountName = Environment.GetEnvironmentVariable("StorageAccountName");
        var credential = new ChainedTokenCredential(
            new ManagedIdentityCredential(),
            new EnvironmentCredential(),
            new AzureCliCredential());
        _logger = loggerFactory.CreateLogger<TableCreatedEventHandlerFunction>();
        if (storageAccountName != null)
        {
            _tableClient = new TableClient(
                new Uri(storageAccountName),
                "users",
                credential);
        }

    }

    [Function("TableCreatedEventHandlerFunction")]
    public void Run([EventGridTrigger] TableCreatedEvent input)
    {
        _logger.LogInformation(input.Data.ToString());

        //var entity = new PlayerTableEntity
        //{
        //    PartitionKey = "player",
        //    RowKey = Guid.NewGuid().ToString(),
        //    Order = 0,
        //    IsDealer = true,
        //    DisplayName = "Dealer",
        //    SessionId = input.Data.SessionId,
        //    UserId = input.Data.UserId,
        //    ETag = ETag.All,
        //    Timestamp = DateTimeOffset.UtcNow
        //};
        //await _tableClient.AddEntityAsync(entity);
    }
}