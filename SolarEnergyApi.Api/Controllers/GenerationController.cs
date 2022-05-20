namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SolarEnergyApi.Domain.Dtos;
    using SolarEnergyApi.Domain.Entities;
    using SolarEnergyApi.Domain.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/plant/{plantId}/generation")]
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

        [HttpPost]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Plant Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Add Generation", Description = "Add generation to plant")]
        public async Task<IActionResult> Post(int plantId, AddGeneration model)
        {
            try
            {
                var plant = await _plantService.GetById(plantId);
                var generation = new Generation(model.Date, model.GeneratePower, plantId);
                await _generationService.Add(generation);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success",
            type: typeof(IEnumerable<ReadGeneration>)
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(
            Summary = "Get generations by plant id",
            Description = "Get generations by plant id"
        )]
        public async Task<IActionResult> Get(int plantId)
        {
            try
            {
                var plant = await _plantService.GetById(plantId);
                var generations = await _generationService.GetAll(plantId);
                return Ok(generations);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{generationId}")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success",
            type: typeof(ReadGeneration)
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Get generation by id", Description = "Get generation by id")]
        public async Task<IActionResult> GetById(int plantId, int generationId)
        {
            try
            {
                var plant = await _plantService.GetById(plantId);
                var generation = await _generationService.GetById(plantId, generationId);
                return Ok(new ReadGeneration(generation));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{generationId}")]
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
            description: "Not Found"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [SwaggerOperation(
            Summary = "Update a specific generation",
            Description = "Updates a specific generation with the given id"
        )]
        public async Task<IActionResult> Put(int plantId, int generationId, UpdateGeneration model)
        {
            try
            {
                var plant = await _plantService.GetById(plantId);
                var generation = await _generationService.GetById(plantId, generationId);
                generation.Update(model.Date, model.GeneratePower);
                await _generationService.Update();
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{generationId}")]
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
            description: "Not Found"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [SwaggerOperation(
            Summary = "Delete a specific generation",
            Description = "Deletes a specific generation with the given id"
        )]
        public async Task<IActionResult> Delete(int plantId, int generationId)
        {
            try
            {
                var plant = await _plantService.GetById(plantId);
                var generation = await _generationService.GetById(plantId, generationId);
                await _generationService.Delete(generation);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("/api/plant/generations-last-12-months")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success",
            type: typeof(IEnumerable<ReadMonthGeneration>)
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
            IEnumerable<ReadMonthGeneration> generations = await _generationService.GetByMonth(
                months
            );
            return Ok(generations);
        }
    }
}
