using RestAPI.Models;
using RestAPI.ParkingLogic;
using System;
using System.Collections.Generic;
using RestAPI.Data;
using System.Linq;

namespace RestAPI.Controllers
{
    public class DbQueries : IDbQueries
    {
        public void CreateReceipt(IReceipt receipt, IParking foundParking, ICalculate calculate, SpaceParkDbContext dbContext)
        {
            receipt.Name = foundParking.CharacterName;
            receipt.Arrival = (DateTime)foundParking.Arrival;
            receipt.StarshipName = foundParking.SpaceshipName;
            receipt.Departure = DateTime.Now;
            receipt.Size = foundParking.Size;

            double diff = (receipt.Departure - receipt.Arrival).TotalMinutes;
            var price = calculate.Price(diff, foundParking.Size.Type);

            receipt.TotalAmount = price;

            dbContext.Receipts.Add((Receipt)receipt);

            foundParking.Arrival = null;
            foundParking.CharacterName = null;
            foundParking.SpaceshipName = null;
            dbContext.SaveChanges();
        }

        public ICollection<Parking> FindUnoccupiedParkings(SpaceParkDbContext dbContext, int id)
        {
            var unoccupiedParkings = (from sp in dbContext.SpacePorts
                join p in dbContext.Parkings
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

            return unoccupiedParkings;
        }
    }
}