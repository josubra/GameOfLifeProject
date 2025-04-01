using GameOfLifeApi.Data;
using GameOfLifeApi.Entity;
using GameOfLifeApi.Service.Abstractions;
using GameOfLifeApi.Util;
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
            var validationResult = ValidateAndLog(id, guid => id > 0, "Invalid board Id received.");
            if (validationResult != null) return validationResult;

            var board = await _gameOfLifeService.GetGameOfLifeBoardAsync(id, cancelationToken);
            if (board == null)
                return NotFound();
            return board;
        }

        [HttpPost("/board")]
        public async Task<ActionResult> Post([FromBody] bool[][] boardState, CancellationToken cancelationToken)
        {
            var validationResult = ValidateAndLog(boardState,HelperUtil.ValidateBoard,
                 "Invalid board state received.");
            if (validationResult != null) return validationResult;

            var newBoard = await _gameOfLifeService.AddGameOfLifeBoardAsync(boardState, cancelationToken);
            return Ok(new { boardId = newBoard.Id });
        }

        [HttpGet("/board/{id}/next")]
        public async Task<ActionResult<bool[][]?>> GetNextState(int id, CancellationToken cancelationToken)
        {
            var validationResult = ValidateAndLog(id, guid => id > 0, "Invalid board Id received.");
            if (validationResult != null) return validationResult;

            var nextState = await _gameOfLifeService.GetAndSaveNextStateAsync(id);
            return nextState != null ? Ok(nextState) : NotFound();
        }

        [HttpGet("/board/{id}/next/{steps}")]
        public async Task<ActionResult<bool[][]?>> GetFutureState(int id, int steps, CancellationToken cancelationToken)
        {
            var validationResult = ValidateAndLog((id, steps), data => data.id > 0 && data.steps > 0,
                $"Invalid parameters for next state. Parameters must be greater than zero.");
            if (validationResult != null) return validationResult;

            var nextState = await _gameOfLifeService.GetAndSaveFutureStateAsync(id, steps);
            return nextState != null ? Ok(nextState) : NotFound();
        }

        [HttpGet("/board/{id}/final/{maxAttempts}")]
        public async Task<ActionResult<bool[][]?>> GetFinalState(int id, int maxAttempts, CancellationToken cancelationToken)
        {
            var validationResult = ValidateAndLog((id, maxAttempts), data => data.id > 0 && data.maxAttempts > 0,
                $"Invalid parameters for next state. Parameters must be greater than zero.");
            if (validationResult != null) return validationResult;

            var finalState = await _gameOfLifeService.GetAndSaveFinalStateAsync(id, maxAttempts);
            return finalState != null ? Ok(finalState) : NotFound();
        }

        //In a big project, this method could be moved to a base controller class
        private ActionResult? ValidateAndLog<T>(T input, Func<T, bool> isValid, string errorMessage)
        {
            if (!isValid(input))
            {
                _logger.LogWarning(errorMessage);
                return BadRequest(errorMessage);
            }
            return null;
        }
    }
}
