using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;
using PHILOBMDatabase.Repositories.Interfaces;

namespace PHILOBMBusiness.Services;

public class CarService : BaseService<Car>, ICarService
{
    private readonly ICarRepository _carRepository;
    public CarService(ICarRepository carRepository) : base(carRepository)
    {

    }

    public async Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId)
    {
        return await _carRepository.GetAllCarsByClientIdAsync(clientId);
    }

    public async Task<Car?> GetCarByIdWithServicesAsync(int carId)
    {
        return await _carRepository.GetCarByIdWithServicesAsync(carId);
    }

}
