namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SolarEnergyApi.Domain.Dtos;
    using SolarEnergyApi.Domain.Entities;
    using SolarEnergyApi.Domain.Interfaces;

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
