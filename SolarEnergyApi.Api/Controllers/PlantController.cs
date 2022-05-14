namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SolarEnergyApi.Domain.Dtos;
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
        public async Task<IActionResult> GetAll(int page, int limit, string? filter, Boolean? active) => Ok(await _plantService.GetAll(page, limit, filter, active));
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var plant = await _plantService.GetById(id);
                return Ok(plant);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(AddPlant model)
        {
            var plant = new Plant(
                model.Nickname,
                model.Place,
                model.Brand,
                model.Model,
                model.Active
            );

            await _plantService.Add(plant);
            return CreatedAtAction(nameof(GetById), new { id = plant.Id }, plant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdatePlant model)
        {
            try
            {
                var plant = await _plantService.GetById(id);
                plant?.Update(model.Nickname, model.Place, model.Brand, model.Model, model.Active);
                await _plantService.Update();
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var plant = await _plantService.GetById(id);
                await _plantService.Delete(plant);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
