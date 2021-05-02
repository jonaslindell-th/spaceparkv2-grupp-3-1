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
    public class ManageParkingsController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;

        public ManageParkingsController(SpaceParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<ManageParkingsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ManageParkingsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManageParkingsController>
        [HttpPost]
        public IActionResult Post([FromBody] Parking parking)
        {
            parking.CharacterName = "";
            parking.SpaceshipName = "";
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

        // PUT api/<ManageParkingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManageParkingsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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
                return StatusCode(StatusCodes.Status404NotFound, "Parking not found.");
            }
        }
    }
}
