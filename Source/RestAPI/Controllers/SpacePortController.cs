using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class SpacePortController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;
        private IReceipt _receipt;
        private IDbFind _dbFind;

        public SpacePortController(SpaceParkDbContext dbContext, IReceipt receipt, IDbFind dbFind)
        {
            _dbContext = dbContext;
            _receipt = receipt;
            _dbFind = dbFind;
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
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var parkings = (from sp in _dbContext.SpacePorts
                join p in _dbContext.Parkings
                    on sp.Id equals p.SpacePortId
                where p.SpacePortId == id && p.CharacterName == null
                select new Parking()
                {
                    Id = p.Id,
                    SizeId = p.SizeId,
                    CharacterName = p.CharacterName,
                    SpaceshipName = p.SpaceshipName,
                    Arrival = p.Arrival,
                    SpacePortId = p.SpacePortId
                }).ToList();

            if (parkings.Count > 0)
            {
                var validPerson = Validate.Person(request.PersonName);
                var validShip = Validate.Starship(request.ShipName);
                //TODO: Tryparse
                var length = double.Parse(validShip.Result.Length);

                var parkingId = _dbFind.VacantParking(length, _dbContext);

                if (validPerson.Result && validShip.Result != null)
                {
                    //TODO: In Vacant parking?
                    var foundParking = _dbContext.Parkings.FirstOrDefault(p => p.Id == parkingId);
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
                        //TODO: Correct status code
                        return StatusCode(StatusCodes.Status404NotFound, "Parking was not found.");
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Not a valid character or ship");
                }
            }
            return StatusCode(StatusCodes.Status423Locked, "SpacePort is full");
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
                    _receipt.Name = foundParking.CharacterName;
                    _receipt.Arrival = (DateTime) foundParking.Arrival;
                    _receipt.StarshipName = foundParking.SpaceshipName;
                    _receipt.Departure = DateTime.Now;
                    _receipt.SizeId = foundParking.SizeId;

                    //TODO: DbQueries.GetSize()
                    var size = (from p in _dbContext.Parkings
                        join s in _dbContext.Sizes
                            on p.SizeId equals s.Id
                        where p.SizeId == foundParking.SizeId
                        select new
                        {
                            s.Id,
                            s.Type
                        }).FirstOrDefault();

                    double diff = (_receipt.Departure - _receipt.Arrival).TotalMinutes;
                    double price = 0;

                    //TODO: Switch expression c# 9.0 in new method CalculatePrice()
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

                    _receipt.TotalAmount = price;

                    _dbContext.Receipts.Add((Receipt)_receipt);


                    foundParking.Arrival = null;
                    foundParking.CharacterName = null;
                    foundParking.SpaceshipName = null;
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
