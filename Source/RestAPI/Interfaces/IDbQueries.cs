using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ParkingLogic;

namespace RestAPI.Controllers
{
    public interface IDbQueries
    {
        void CreateReceipt(IReceipt receipt, IParking foundParking, ICalculate calculate, SpaceParkDbContext dbContext);
    }
}