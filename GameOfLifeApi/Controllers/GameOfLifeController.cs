using GameOfLifeApi.Data;
using GameOfLifeApi.Entity;
using GameOfLifeApi.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<GameOfLifeBoard>> Get(int id, CancellationToken cancelationToken)
        {
            var board = await _gameOfLifeService.GetGameOfLifeBoardAsync(id, cancelationToken);
            if (board == null)
                return NotFound();
            return board;
        }
    }
}
