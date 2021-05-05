﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSpacePorts : ControllerBase
    {
        private SpaceParkDbContext _dbContext;
        private ISpacePort _spacePort { get; set; }
        private IParking _parking { get; set; }

        public ManageSpacePorts(SpaceParkDbContext context, ISpacePort spacePort, IParking parking)
        {
            _dbContext = context;
            _spacePort = spacePort;
            _parking = parking;
        }

        // GET: api/<ManageSpacePorts>
        [HttpGet]
        public IActionResult Get()
        {
            var spacePorts = _dbContext.SpacePorts.Include("Parkings").ToList();
            
            return Ok(spacePorts);
        }

        // POST api/<ManageSpacePorts>
        [HttpPost]
        public IActionResult Post([FromBody] string name)
        {
            try
            {
                _spacePort.Name = name;
                _spacePort.Parkings = new List<Parking>();
                for (int i = 0; i < 5; i++)
                {
                    _parking = new Parking();
                    _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.Small).Select(s => s.Id).FirstOrDefault();
                    _spacePort.Parkings.Add((Parking)_parking);
                }                
                for (int i = 0; i < 3; i++)
                {
                    _parking = new Parking();
                    _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.Medium).Select(s => s.Id).FirstOrDefault();
                    _spacePort.Parkings.Add((Parking)_parking);
                }                
                for (int i = 0; i < 2; i++)
                {
                    _parking = new Parking();
                    _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.Large).Select(s => s.Id).FirstOrDefault();
                    _spacePort.Parkings.Add((Parking)_parking);
                }

                _parking = new Parking();
                _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.VeryLarge).Select(s => s.Id).FirstOrDefault();
                _spacePort.Parkings.Add((Parking)_parking);
                _dbContext.SpacePorts.Add((SpacePort)_spacePort);

                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, $"SpacePort added id: {_spacePort.Id} name: {_spacePort.Name}");
            }
            catch
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Not authorized");
            }
        }

        // DELETE api/<ManageSpacePorts>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
