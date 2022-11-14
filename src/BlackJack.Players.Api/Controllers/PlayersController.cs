using BlackJack.Players.Core.Abstractions.DataTransferObjects;
using BlackJack.Players.Core.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlackJack.Players.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post(PlayerCreateDto dto, [FromServices] IBlackJackPlayersService playersService)
        {
            try
            {
                var details = await playersService.CreateAsync(dto);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
