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
        [HttpPut("[action]/{id}")]
        public IActionResult Park(int id, [FromBody] ParkRequest request)
        {
            var validPerson = Validate.Person(request.PersonName);
            var validShip = Validate.Starship(request.ShipName);
            if (validPerson.Result && validShip.Result != null)
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

        // PUT api/<SpaceParkController>/5
        [HttpPut("[action]/{id}")]
        public IActionResult Unpark(int id, [FromBody] ParkRequest request)
        {
            var foundParking = _dbContext.Parkings.FirstOrDefault(p => p.Id == id);
            if (foundParking != null)
            {
                if (foundParking.CharacterName == request.PersonName && foundParking.SpaceshipName == request.ShipName)
                {

                    Receipt receipt = new Receipt();
                    receipt.Name = foundParking.CharacterName;
                    receipt.Arrival = (DateTime) foundParking.Arrival;
                    receipt.StarshipName = foundParking.SpaceshipName;
                    receipt.Departure = DateTime.Now;
                    receipt.SizeId = foundParking.SizeId;

                    var size = (from p in _dbContext.Parkings
                        join s in _dbContext.Sizes
                            on p.SizeId equals s.Id
                        where p.SizeId == foundParking.SizeId
                        select new
                        {
                            s.Id,
                            s.Type
                        }).FirstOrDefault();

                    double diff = (receipt.Departure - receipt.Arrival).TotalMinutes;
                    double price = 0;
                    // Then calculate the minute price of parkingize times the amount of minutes + the starting fee.
                    if (size.Type == ParkingSize.Small)
                    {
                        price = (Math.Round(diff, 0) * 200) + 100;
                    }
                    else if (size.Type == ParkingSize.Medium)
                    {
                        price = (Math.Round(diff, 0) * 800) + 400;
                    }
                    else if (size.Type == ParkingSize.Large)
                    {
                        price = (Math.Round(diff, 0) * 1800) + 900;
                    }
                    else
                    {
                        price = (Math.Round(diff, 0) * 12000) + 6000;
                    }

                    receipt.TotalAmount = price;

                    _dbContext.Receipts.Add(receipt);


                    foundParking.Arrival = null;
                    foundParking.CharacterName = "";
                    foundParking.SpaceshipName = "";
                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, $"Vehicle unparked, total cost: {price}.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Incorrect person or ship");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Parking was not found.");
            }
        }

        // DELETE api/<SpaceParkController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
