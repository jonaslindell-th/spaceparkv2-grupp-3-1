using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class DbFind : IDbFind
    {
        public int VacantParking(double shipLength, int spacePortId, SpaceParkDbContext context)
        {
            var size = shipLength switch
            {
                < 500 => ParkingSize.Small,
                < 1000 => ParkingSize.Medium,
                < 10000 => ParkingSize.Large,
                 _ => ParkingSize.VeryLarge
            };

            //var parkingList = context.Parkings.FirstOrDefault(p => p.Size.Type == size && string.IsNullOrEmpty(p.CharacterName));
            var parking = context.Parkings.Include("Size").FirstOrDefault(p => p.Size.Type == size && string.IsNullOrEmpty(p.CharacterName) && p.SpacePortId == spacePortId);


            if (parking != null)
            {
                return parking.Id;
            }

            return 0;
        }
    }
}
