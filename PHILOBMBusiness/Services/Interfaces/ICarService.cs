using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services.Interfaces;

public interface ICarService : IBaseService<Car>
{
    Task<ICollection<Car>> GetAllCarsWithClientAsync();
    Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId);
    Task<Car?> GetCarByIdWithServicesAsync(int carId);
}
