using RestAPI.Models;
using RestAPI.ParkingLogic;
using System;
using RestAPI.Data;

namespace RestAPI.Controllers
{
    public class DbQueries : IDbQueries
    {
        public void CreateReceipt(IReceipt receipt, Parking foundParking, ICalculate calculate, SpaceParkDbContext dbContext)
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
    }
}