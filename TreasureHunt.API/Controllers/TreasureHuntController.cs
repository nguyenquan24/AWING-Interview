using Microsoft.AspNetCore.Mvc;
using TreasureHunt.Core.Entities;
using TreasureHunt.Core.Interfaces;
using TreasureHunt.Core.Models;

namespace TreasureHunt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreasureHuntController : ControllerBase
    {
        private readonly ITreasureHuntService _treasureHuntService;

        public TreasureHuntController(ITreasureHuntService treasureHuntService)
        {
            _treasureHuntService = treasureHuntService;
        }

        [HttpPost("solve")]
        public async Task<ActionResult<double>> SolveTreasureHunt([FromBody] TreasureMapRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _treasureHuntService.SolveTreasureHuntAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreasureMap>>> GetHistory()
        {
            var history = await _treasureHuntService.GetHistoryAsync();
            return Ok(history);
        }
    }
}