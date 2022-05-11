namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SolarEnergyApi.Data.Dtos;
    using SolarEnergyApi.Domain.Entities;
    using SolarEnergyApi.Domain.Interfaces;

    [Route("api/plant")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly IPlantService _plantService;
        
        public PlantController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet()]
        public IActionResult GetAll(int page, string? filter, Boolean? active)
        {
            var plants = _plantService.GetAll(page, 10, filter, active);
            return Ok(plants);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var plant = _plantService.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }
            return Ok(plant);
        }

        [HttpPost]
        public IActionResult Post(AddPlant model)
        {
            var plant = new Plant(
                model.Nickname,
                model.Place,
                model.Brand,
                model.Model,
                model.Active
            );

            _plantService.Add(plant);
            return CreatedAtAction(nameof(GetById), new { id = plant.Id }, plant);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdatePlant model)
        {
            var plant = _plantService.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }
            plant.Update(model.Nickname, model.Place, model.Brand, model.Model, model.Active);
            _plantService.Update();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var plant = _plantService.GetById(id);
            if (plant == null)
            {
                return NotFound();
            }
            _plantService.Delete(plant);
            return NoContent();
        }
    }
}
