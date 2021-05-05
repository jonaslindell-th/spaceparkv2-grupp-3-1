namespace RestAPI.Data
{
    public interface IDbFind
    {
        int CorrectSizeParking(double shipLength, int spacePortId, SpaceParkDbContext context);
    }
}