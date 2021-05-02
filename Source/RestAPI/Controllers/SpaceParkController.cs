using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.Requests;
using RestAPI.Swapi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("/api/[controller]")]
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SpaceParkController>/5
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
        [HttpPut("park/{id}")]
        public IActionResult Park(int id, [FromBody] ParkRequest request)
        {
            var validPerson = Validate.Person(request.PersonName);
            var validShip = Validate.Starship(request.ShipName);
            if (validPerson.Result && validShip.Result)
            {
                var foundParking = _dbContext.Parkings.FirstOrDefault(p => p.Id == id);
                if (foundParking != null)
                {
                    foundParking.Arrival = DateTime.Now;
                    foundParking.CharacterName = request.PersonName;
                    foundParking.SpaceshipName = request.ShipName;
                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, "Vehicle parked.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Parking was not found.");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Not a valid character or ship");
            }
        }

        // DELETE api/<SpaceParkController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
