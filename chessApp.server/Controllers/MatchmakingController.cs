using ChessApp.Services;
using ChessApp.commonModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using chessApp.server.services;

namespace ChessApp.Controllers
{
    [Route("api/matchmaking")]
    [ApiController]
    public class MatchmakingController : ControllerBase
    {
        private readonly MatchmakingService _matchmakingService;

        public MatchmakingController(MatchmakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }

        [HttpPost("enqueue")]
        public async Task<IActionResult> EnqueuePlayer([FromBody] MatchmakingQueue queueEntry)
        {
            await _matchmakingService.AddToQueueAsync(queueEntry);
            return Ok("Player added to matchmaking queue.");
        }

        [HttpGet("find/{rating}/{timeControl}")]
        public async Task<IActionResult> FindMatch(int rating, string timeControl)
        {
            var match = await _matchmakingService.FindMatchAsync(rating, timeControl);
            if (match == null)
                return NotFound("No match found.");

            await _matchmakingService.RemoveFromQueueAsync(match.UserId);
            return Ok(match);
        }
    }
}
