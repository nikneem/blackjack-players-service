using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Messaging;
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

    [Function(nameof(TableCreatedEventHandlerFunction))]
    public void Run([EventGridTrigger] CloudEvent e)
    {
        _logger.LogInformation("Event received {type} {subject}", e.Type, e.Subject);
    }
}