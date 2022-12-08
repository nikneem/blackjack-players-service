﻿using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using BlackJack.Events;
using BlackJack.Events.EventData;
using BlackJack.Players.Core.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlackJack.Players.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHooksController : ControllerBase
    {

        [HttpPost("session_created_webhook")]
        public async Task<IActionResult> SessionCreatedWebhook(
            [FromBody] CloudEvent[] ev,
            [FromServices] IBlackJackPlayersService blackJackPlayersService,
            [FromServices] ILogger<WebHooksController> logger)
        {
            foreach (var eventGridEvent in ev)
            {
                logger.LogInformation("Received webhook event {event}", JsonConvert.SerializeObject(eventGridEvent));
                if (eventGridEvent.Type == SystemEventNames.EventGridSubscriptionValidation &&
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

                if (eventGridEvent.Type == BlackJackEventNames.SessionCreated &&
                    eventGridEvent.Data != null)
                {
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
