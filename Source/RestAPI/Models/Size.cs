using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class Size
    {
        public int Id { get; set; }
        public ParkingSize Type { get; set; } 
    }
    public enum ParkingSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
        VeryLarge = 4
    }
}
