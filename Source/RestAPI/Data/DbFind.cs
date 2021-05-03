using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data
{
    public class DbFind : IDbFind
    {
        public int VacantParking(double shipLength, SpaceParkDbContext context)
        {
            var size = shipLength switch
            {
                < 500 => 1,
                < 1000 => 2,
                < 10000 => 3,
                 _ => 4
            };

            var parking = context.Parkings.FirstOrDefault(p => p.SizeId == size && string.IsNullOrEmpty(p.CharacterName));

            if (parking != null)
            {
                return parking.Id;
            }

            return 0;
        }
    }
}
