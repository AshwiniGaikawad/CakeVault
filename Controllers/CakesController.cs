using CakeVault.Models;
using CakeVault.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CakesController : ControllerBase
    {
        private readonly ICakeVaultRepository _cakeVaultRepository;

        public CakesController(ICakeVaultRepository cakeVaultRepository)
        {
            _cakeVaultRepository = cakeVaultRepository;
        }

        // GET: api/cakes
        [HttpGet]
        public async Task<ActionResult> GetCakes()
        {
            try
            {
                var cakes = await _cakeVaultRepository.GetAllCakesAsync();
                return cakes.Any() ? Ok(cakes) : NotFound("No cakes found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/cakes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCake(int id)
        {
            try
            {
                var cake = await _cakeVaultRepository.GetCakeByIdAsync(id);

                if (cake == null)
                {
                    return NotFound($"Cake with ID {id} not found.");
                }

                return Ok(cake);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // POST: api/cakes
        [HttpPost]
        public async Task<ActionResult> PostCake(CakeDTO cake)
        {
            try
            {
                // Check if the model is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _cakeVaultRepository.CakeExistsByNameAsync(cake.Name))
                    return BadRequest($"A cake with the name '{cake.Name}' already exists.");

                var newCake = new Cake
                {
                    Name = cake.Name,
                    Comment = cake.Comment,
                    ImageUrl = cake.ImageUrl,
                    YumFactor = cake.YumFactor
                };
                await _cakeVaultRepository.AddCakeAsync(newCake);
                await _cakeVaultRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCake), new { id = newCake.Id }, newCake);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        // PUT: api/cakes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCake(int id, Cake cake)
        {
            try
            {
                if (id != cake.Id)
                {
                    return BadRequest("The cake ID in the URL does not match the cake ID in the request body.");
                }


                var existingCake = await _cakeVaultRepository.GetCakeByIdAsync(id);
                if (existingCake == null)
                    return NotFound($"Cake with ID {id} not found.");

                // If the name hasn't changed, don't check for name uniqueness
                if (!existingCake.Name.Equals(cake.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var nameExists = await _cakeVaultRepository.CakeExistsByNameAsync(cake.Name);

                    if (nameExists)
                    {
                        return BadRequest($"A cake with the name '{cake.Name}' already exists.");
                    }
                }


                await _cakeVaultRepository.UpdateCakeAsync(cake);
                await _cakeVaultRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }




        // DELETE: api/cakes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCake(int id)
        {
            try
            {
                var cake = await _cakeVaultRepository.GetCakeByIdAsync(id);
                if (cake == null)
                    return NotFound($"Cake with ID {id} not found.");

                await _cakeVaultRepository.DeleteCakeAsync(cake);
                await _cakeVaultRepository.SaveChangesAsync();

                return NoContent(); // 204 - Successfully deleted
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}