namespace RestAPI.Data
{
    public interface IDbFind
    {
        int VacantParking(double shipLength, int spacePortId, SpaceParkDbContext context);
    }
}