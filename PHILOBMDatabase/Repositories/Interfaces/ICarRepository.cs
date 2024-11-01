using PHILOBMCore.Models;

namespace PHILOBMDatabase.Repositories.Interfaces;

public interface ICarRepository : IBaseRepository<Models.Car>
{
    Task<ICollection<Car>> GetAllCarsWithClientAsync();
    Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId);
    Task<Car?> GetCarByIdWithServicesAsync(int carId);
}
