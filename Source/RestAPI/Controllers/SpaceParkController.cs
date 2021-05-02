using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestAPI.Swapi;
using RestAPI.Data;
using RestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceParkController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;

        public SpaceParkController(SpaceParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<SpaceParkController>
        [HttpGet]
        public IActionResult Get([FromBody] string name)
        {
            var isValid = Validate.Person(name);
            if (isValid.Result)
            {
             return StatusCode(StatusCodes.Status200OK, "Person Validated");
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Not a authorized person");
            }
        }

        //GET api/<SpaceParkController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SpaceParkController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SpaceParkController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SpaceParkController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // POST api/<ManageParkingsController>
        [HttpPost("[action]")]
        public IActionResult AddParkingspots([FromBody] Parking parking)
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

        // DELETE api/<ManageParkingsController>/5
        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteParkingspots(int id)
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
