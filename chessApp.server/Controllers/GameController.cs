using ChessApp.commonModels;
using ChessApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessApp.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetAllGames()
        {
            return Ok(await _gameService.GetAllGamesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(string id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(Game game)
        {
            var createdGame = await _gameService.CreateGameAsync(game);
            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(string id, Game updatedGame)
        {
            var success = await _gameService.UpdateGameAsync(id, updatedGame);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(string id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
