namespace RestAPI.Data
{
    public interface IDbFind
    {
        int VacantParking(double shipLength, SpaceParkDbContext context);
    }
}