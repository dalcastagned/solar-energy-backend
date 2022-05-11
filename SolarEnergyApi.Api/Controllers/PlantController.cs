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
        public IActionResult GetAll(int page, string? filter, Boolean? active) => Ok(_plantService.GetAll(page, 10, filter, active));
       
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var plant = _plantService.GetById(id);
                return Ok(plant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
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
            try
            {
                var plant = _plantService.GetById(id);
                plant.Update(model.Nickname, model.Place, model.Brand, model.Model, model.Active);
                _plantService.Update();
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var plant = _plantService.GetById(id);
                _plantService.Delete(plant);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
