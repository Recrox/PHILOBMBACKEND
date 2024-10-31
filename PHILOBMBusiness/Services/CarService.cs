using AutoMapper;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;
using PHILOBMDatabase.Repositories.Interfaces;

namespace PHILOBMBusiness.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public CarService(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task AddAsync(Car entity)
    {
        var carEntity = _mapper.Map<PHILOBMDatabase.Models.Car>(entity);
        await _carRepository.AddAsync(carEntity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _carRepository.DeleteAsync(id);
    }

    public async Task<ICollection<Car>> GetAllAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return _mapper.Map<ICollection<Car>>(cars);
    }

    public async Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId)
    {
        var cars = await _carRepository.GetAllCarsByClientIdAsync(clientId);
        return _mapper.Map<ICollection<Car>>(cars);
    }

    public async Task<Car?> GetByIdAsync(int id)
    {
        var car = await _carRepository.GetByIdAsync(id);
        return _mapper.Map<Car?>(car);
    }

    public async Task<Car?> GetCarByIdWithServicesAsync(int carId)
    {
        var carWithServices = await _carRepository.GetCarByIdWithServicesAsync(carId);
        return _mapper.Map<Car?>(carWithServices);
    }

    public async Task UpdateAsync(Car entity)
    {
        var carEntity = _mapper.Map<PHILOBMDatabase.Models.Car>(entity);
        await _carRepository.UpdateAsync(carEntity);
    }
}
