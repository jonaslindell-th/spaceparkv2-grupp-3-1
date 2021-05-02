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

      
    }
}
