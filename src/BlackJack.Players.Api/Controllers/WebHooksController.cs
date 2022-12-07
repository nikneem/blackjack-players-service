using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using BlackJack.Events;
using BlackJack.Events.EventData;
using BlackJack.Players.Core.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlackJack.Players.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHooksController : ControllerBase
    {

        [HttpPost("session_created_webhook")]
        public async Task<IActionResult> SessionCreatedWebhook(
            [FromBody] EventGridEvent[] ev,
            [FromServices] IBlackJackPlayersService blackJackPlayersService,
            [FromServices] ILogger<WebHooksController> logger)
        {
            foreach (var eventGridEvent in ev)
            {
                if (eventGridEvent.EventType == SystemEventNames.EventGridSubscriptionValidation &&
                    eventGridEvent.Data != null)
                {
                    logger.LogInformation("Receiving subscription validation data");
                    var subscriptionValidationData = eventGridEvent.Data.ToObjectFromJson<SubscriptionValidationEventData>();
                    var response = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = subscriptionValidationData.ValidationCode
                    };
                    logger.LogInformation("Returning subscription validation response");
                    return Ok(response);
                }

                if (eventGridEvent.EventType == BlackJackEventNames.SessionCreated &&
                    eventGridEvent.Data != null)
                {
                    logger.LogInformation("Received session created event, now creating a dealer for this session");
                    var eventData = eventGridEvent.Data.ToObjectFromJson<TableCreatedEventData>();
                    var dealerCreated = await blackJackPlayersService.CreateDealerAsync(eventData.UserId, eventData.SessionId);
                    logger.LogInformation("Session dealer created -> {success}", dealerCreated);
                    if (!dealerCreated)
                    {
                        return BadRequest();
                    }
                }
            }

            return Ok();
        }

    }
}
