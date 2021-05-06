using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ParkingLogic;
using RestAPI.Requests;
using RestAPI.Swapi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;
        private IReceipt _receipt;
        private IDbFind _dbFind;
        private ICalculate _calculate;
        private IDbQueries _dbQueries;

        public UserController(SpaceParkDbContext dbContext, IReceipt receipt, IDbFind dbFind, ICalculate calculate, IDbQueries dbQueries)
        {
            _dbContext = dbContext;
            _receipt = receipt;
            _dbFind = dbFind;
            _calculate = calculate;
            _dbQueries = dbQueries;
        }

        // GET: api/<SpaceParkController>
        [HttpGet("[action]")]
        public IActionResult ParkingHistory([FromBody] string name)
        {
            var receipts = _dbContext.Receipts.Where(r => r.Name.ToLower() == name.ToLower()).ToList();

            return Ok(receipts);
        }

        // GET api/<SpaceParkController>/5
        [HttpGet("[action]")]
        public IActionResult ActiveParkings([FromBody] string name)
        {
            var activeParkings = _dbContext.Parkings.Where(p => p.CharacterName.ToLower() == name.ToLower()).ToList();

            return Ok(activeParkings);
        }

        // PUT api/<SpaceParkController>/5
        [HttpPut("[action]/{id}")]
        public IActionResult Park(int id, [FromBody] ParkRequest request)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var validSpacePort = _dbContext.SpacePorts.FirstOrDefault(sp => sp.Id == id);
            if (validSpacePort == null)
            {
                return BadRequest($"There is no existing spaceport with id:{id}.");
            }

            var unoccupiedParkings = _dbQueries.FindUnoccupiedParkings(_dbContext, id);

            if (unoccupiedParkings.Count > 0)
            {
                var validPerson = Validate.Person(request.PersonName);
                var validShip = Validate.Starship(request.ShipName);
                //TODO: Tryparse
                var length = double.Parse(validShip.Result.Length);

                var parkingId = _dbFind.CorrectSizeParking(length, id, _dbContext);

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
                    return BadRequest("No suitable parking was found for your ship length.");
                }
                return StatusCode(StatusCodes.Status401Unauthorized, "Not a valid character or ship");
            }
            //return StatusCode(StatusCodes.Status423Locked, "SpacePort is full");
            return BadRequest("Space port is full.");
        }

        // PUT api/<SpaceParkController>/5
        [HttpPut("[action]/{id}")]
        public IActionResult Unpark(int id, [FromBody] ParkRequest request)
        {
            IParking foundParking = _dbContext.Parkings.Include(p => p.Size).FirstOrDefault(p => p.Id == id && request.PersonName.ToLower() == p.CharacterName.ToLower() && request.ShipName.ToLower() == p.SpaceshipName.ToLower());
            
            if (foundParking != null)
            {
                _dbQueries.CreateReceipt(_receipt, foundParking, _calculate, _dbContext);

                return StatusCode(StatusCodes.Status200OK, $"Vehicle unparked.");
            }
            return BadRequest("Incorrect name, ship or parking spot id");
        }
    }
}

