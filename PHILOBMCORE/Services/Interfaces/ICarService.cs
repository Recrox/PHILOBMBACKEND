using PHILOBMCore.Models;
namespace PHILOBMCore.Services.Interfaces;

public interface ICarService : IBaseContextService<Car>
{
    Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId);
    Task<Car?> GetCarByIdWithServicesAsync(int carId);
}
