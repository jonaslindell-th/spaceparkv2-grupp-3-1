using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ParkingLogic;

namespace RestAPI.Controllers
{
    public interface IDbQueries
    {
        void CreateReceipt(IReceipt receipt, Parking foundParking, ICalculate calculate, SpaceParkDbContext dbContext);
    }
}