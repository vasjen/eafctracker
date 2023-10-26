namespace Eafctracker.Services.Interfaces
{
    public interface IPriceService
    {
         Task<int[]> GetPriceAsync (int FBDataId);
         
    }
}