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
    public class ParkController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;

        public ParkController(SpaceParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<ParkController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ParkController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ParkController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ParkController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ParkRequest request)
        {
            var isValid = Validate.Person(request.PersonName);
            if (isValid.Result)
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

        // DELETE api/<ParkController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
