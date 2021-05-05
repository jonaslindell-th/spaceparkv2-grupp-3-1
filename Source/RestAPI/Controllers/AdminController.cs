using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestAPI.Data;
using RestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;

        public AdminController(SpaceParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST api/Admin/AddParking
        [HttpPost("[action]")]
        public IActionResult AddParking([FromBody] Parking parking)
        {
            parking.CharacterName = null;
            parking.SpaceshipName = null;
            try
            {
                _dbContext.Parkings.Add(parking);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, "Parking Added to Database.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, "An error occurred while adding new parking spot.");
            }
        }

        // DELETE api/Admin/RemoveParking/5
        [HttpDelete("[action]/{id}")]
        public IActionResult RemoveParking(int id)
        {
            var parking = _dbContext.Parkings.FirstOrDefault(p => p.Id == id);
            if (parking != null)
            {
                _dbContext.Parkings.Remove(parking);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"Parking with id:{parking.Id} was removed successfully.");
            }
            else
            {
                return BadRequest($"Parking with id:{id} was not found.");
            }
        }
    }
}
