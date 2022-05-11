namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SolarEnergyApi.Data.Dtos;
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
        public IActionResult Post(int id, AddGeneration model)
        {
            var plant = _plantService.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }

            var generation = new Generation(model.Date, model.GeneratePower, id);
            _generationService.AddGeneration(generation);
            return NoContent();
        }

        [Route("api/plant/generations-last-12-months")]
        [HttpGet]
        public IActionResult GetAllByLast12Months()
        {
            IEnumerable<String> months = Enumerable
                .Range(0, 12)
                .Select(i => DateTimeOffset.Now.AddMonths(-i).ToString("MM/yy"));
            List<Object> generations = _generationService.GetGenerationsByMonth(months);
            return Ok(generations);
        }
    }
}
