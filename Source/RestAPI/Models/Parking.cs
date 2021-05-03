using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        public int SizeId { get; set; }
        public int SpacePortId { get; set; }
        public string CharacterName { get; set; }
        public string SpaceshipName { get; set; }
        public DateTime? Arrival { get; set; }
    }

}
