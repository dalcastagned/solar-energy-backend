namespace SolarPlant.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SolarEnergyApi.Domain.Dtos;
    using SolarEnergyApi.Domain.Entities;
    using SolarEnergyApi.Domain.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("api/plant")]
    [ApiController]
    [Produces("application/json")]
    public class PlantController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet()]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success",
            type: typeof(ReadPlants)
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
            Summary = "Return list of paginated plants",
            Description = "Returns list of paginated plants with optional page, limit, active (search if the plant is active or not) and filter query params (search if the filter is contained in the plant's nickname, brand, model or location)"
        )]
        public async Task<IActionResult> GetAll(
            int page,
            int limit,
            string? filter,
            Boolean? active
        ) => Ok(await _plantService.GetAll(page, limit, filter, active));

        [HttpGet("{id}")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Success",
            type: typeof(Plant)
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
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(
            Summary = "Return a specific plant",
            Description = "Returns a specific plant with the given id"
        )]
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
        [SwaggerResponse(
            statusCode: StatusCodes.Status201Created,
            description: "Created"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(
            Summary = "Create a new plant",
            Description = "Creates a new plant with the given plant model"
        )]
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
            return CreatedAtAction(nameof(GetById), new { id = plant.Id }, plant.Id);
        }

        [HttpPut("{id}")]
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
            Summary = "Update a specific plant",
            Description = "Updates a specific plant with the given id"
        )]        
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
            Summary = "Delete a specific plant",
            Description = "Deletes a specific plant with the given id"
        )]
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
