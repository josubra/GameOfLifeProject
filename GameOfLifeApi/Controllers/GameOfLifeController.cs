using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameOfLifeController : ControllerBase
    {

        private readonly ILogger<GameOfLifeController> _logger;

        public GameOfLifeController(ILogger<GameOfLifeController> logger)
        {
            _logger = logger;
        }
    }
}
