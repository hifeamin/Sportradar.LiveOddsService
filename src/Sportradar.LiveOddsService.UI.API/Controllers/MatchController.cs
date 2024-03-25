using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sportradar.LiveOddsService.Domain.Exceptions;
using Sportradar.LiveOddsService.Domain.Models;
using Sportradar.LiveOddsService.Domain.Services;
using Sportradar.LiveOddsService.UI.API.Models;

namespace Sportradar.LiveOddsService.UI.API.Controllers {
    [ApiController]
    [Route("match")]
    public class MatchController: ControllerBase {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;

        public MatchController(IMatchService matchService, IMapper mapper) {
            _matchService = matchService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Match), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> StartAsync([FromBody] InputMatchModel input) {
            try {
                var result = await _matchService.StartAsync(input.HomeTeam, input.AwayTeam);
                return Ok(result);
            } catch(NullReferenceException ex) when(ex.Message.EndsWith("should be filled!")) {
                return BadRequest(new ProblemDetails() {
                    Title = "Invalid request",
                    Detail = ex.Message
                });
            } catch(ItemAlreadyExistException<Match> ex) {
                return Conflict(new ProblemDetails() {
                    Title = "Coflict data",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateMatchModel input) {
            try {
                Match match = _mapper.Map<Match>(input);
                await _matchService.UpdateAsync(match);
                return Ok();
            } catch(KeyNotFoundException ex) {
                return BadRequest(new ProblemDetails() {
                    Title = "Not Found!",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FinishAsync([FromBody] InputMatchModel inputMatch) {
            try {
                await _matchService.FinishAsync(inputMatch.HomeTeam, inputMatch.AwayTeam);
                return Ok();
            } catch(KeyNotFoundException ex) {
                return BadRequest(new ProblemDetails() {
                    Title = "Not Found!",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Match>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSummeryAsync() {
            var result = await _matchService.GetSummeryAsync();
            return Ok(result);
        }
    }
}
