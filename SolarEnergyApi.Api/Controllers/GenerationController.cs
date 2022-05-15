namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SolarEnergyApi.Domain.Dtos;
    using SolarEnergyApi.Domain.Entities;
    using SolarEnergyApi.Domain.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;

    [ApiController]
    public class GenerationController : ControllerBase
    {
        private readonly IPlantService _plantService;
        private readonly IGenerationService _generationService;

        public GenerationController(
            IPlantService plantService,
            IGenerationService generationService
        )
        {
            _plantService = plantService;
            _generationService = generationService;
        }

        [Route("api/plant/{id}/generation")]
        [HttpPost]
        [SwaggerResponse(
            statusCode: StatusCodes.Status204NoContent,
            description: "No Content"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status404NotFound,
            description: "Plant Not Found"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(
            Summary = "Add Generation",
            Description = "Add generation to plant"
        )]
        public async Task<IActionResult> Post(int id, AddGeneration model)
        {
            try
            {
                var plant = await _plantService.GetById(id);
                var generation = new Generation(model.Date, model.GeneratePower, id);
                await _generationService.AddGeneration(generation);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Route("api/plant/generations-last-12-months")]
        [HttpGet]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(
            Summary = "Last 12 months generation",
            Description = "Last 12 months generation for all plants"
        )]
        public async Task<IActionResult> GetAllByLast12Months()
        {
            IEnumerable<String> months = Enumerable
                .Range(0, 12)
                .Select(i => DateTimeOffset.Now.AddMonths(-i).ToString("MM/yy"));
            IEnumerable<Object> generations = await _generationService.GetGenerationsByMonth(months);
            return Ok(generations);
        }
    }
}
