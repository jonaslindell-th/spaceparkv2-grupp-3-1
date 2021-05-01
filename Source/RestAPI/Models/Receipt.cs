using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        public string Name { get; set; }
        public string StarshipName { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public double TotalAmount { get; set; }
    }
}
