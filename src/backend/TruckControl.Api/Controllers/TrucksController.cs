using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TruckControl.Application.DTOs;
using TruckControl.Application.Service;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrucksController : ControllerBase
{
    private readonly ITruckService _service;

    public TrucksController(ITruckService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<TruckResponseDTO>> Create(TruckRequestDTO request)
    {
        try
        {
            if (request == null)
                return BadRequest("Request cannot be null");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while creating the truck");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TruckResponseDTO>> Update(int id, TruckRequestDTO request)
    {
        try
        {
            if (request == null)
                return BadRequest("Request cannot be null");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _service.UpdateAsync(id, request);
            return response == null ? NotFound() : Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the truck");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TruckResponseDTO>> GetById(int id)
    {
        try
        {
            var response = await _service.GetByIdAsync(id);
            return response == null ? NotFound() : Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving the truck");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TruckResponseDTO>>> GetAll()
    {
        try
        {
            return Ok(await _service.GetAllAsync());
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving trucks");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the truck");
        }
    }
}
