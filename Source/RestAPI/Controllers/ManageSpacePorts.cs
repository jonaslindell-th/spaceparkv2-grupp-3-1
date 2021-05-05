using Microsoft.AspNetCore.Mvc;
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

        public ManageSpacePorts(SpaceParkDbContext context, ISpacePort spacePort)
        {
            _dbContext = context;
            _spacePort = spacePort;
        }

        // GET: api/<ManageSpacePorts>
        [HttpGet]
        //public ICollection<SpacePort> Get()
        public IActionResult Get()
        {
            var spacePorts = _dbContext.SpacePorts.Include("Parkings").ToList();
            
            return Ok(spacePorts);
        }

        // GET api/<ManageSpacePorts>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManageSpacePorts>
        [HttpPost]
        public IActionResult Post([FromBody] string name)
        {
            try
            {
                _spacePort.Name = name;
                _dbContext.SpacePorts.Add((SpacePort)_spacePort);
                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, $"SpacePort added id: {_spacePort.Id} name: {_spacePort.Name}");
            }
            catch
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Not authorized");
            }
        }

        // PUT api/<ManageSpacePorts>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManageSpacePorts>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
