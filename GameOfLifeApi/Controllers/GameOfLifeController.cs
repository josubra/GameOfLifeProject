using GameOfLifeApi.Data;
using GameOfLifeApi.Entity;
using GameOfLifeApi.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameOfLifeController : ControllerBase
    {

        private readonly ILogger<GameOfLifeController> _logger;
        private readonly IGameOfLifeService _gameOfLifeService;

        public GameOfLifeController(ILogger<GameOfLifeController> logger, IGameOfLifeService gameOfLifeService)
        {
            _logger = logger;
            _gameOfLifeService = gameOfLifeService;
        }

        [HttpGet("/board")]
        public async Task<ActionResult<GameOfLifeBoard>> Get(int id, CancellationToken cancelationToken)
        {
            var board = await _gameOfLifeService.GetGameOfLifeBoardAsync(id, cancelationToken);
            if (board == null)
                return NotFound();
            return board;
        }

        [HttpPost("/board")]
        public async Task<int> Post([FromBody] bool[][] boardState, CancellationToken cancelationToken)
        {
            var newBoard = await _gameOfLifeService.AddGameOfLifeBoardAsync(boardState, cancelationToken);
            return newBoard.Id;
        }

        [HttpGet("/board/{id}/next")]
        public async Task<ActionResult<bool[][]?>> GetNextState(int id, CancellationToken cancelationToken)
        {
            var nextState = await _gameOfLifeService.GetAndSaveNextStateAsync(id);
            return nextState != null ? Ok(nextState) : NotFound();
        }

        [HttpGet("/board/{id}/next/{steps}")]
        public async Task<ActionResult<bool[][]?>> GetFutureState(int id, int steps, CancellationToken cancelationToken)
        {
            var nextState = await _gameOfLifeService.GetAndSaveFutureStateAsync(id, steps);
            return nextState != null ? Ok(nextState) : NotFound();
        }

        [HttpGet("/board/{id}/final/{maxAttempts}")]
        public async Task<ActionResult<bool[][]?>> GetFinalState(int id, int maxAttempts, CancellationToken cancelationToken)
        {
            var finalState = await _gameOfLifeService.GetAndSaveFinalStateAsync(id, maxAttempts);
            return finalState != null ? Ok(finalState) : NotFound();
        }
    }
}
