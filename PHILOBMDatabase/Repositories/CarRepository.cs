using Microsoft.EntityFrameworkCore;
using PHILOBMDatabase.Repositories.Interfaces;
using AutoMapper;
using PHILOBMDatabase.Database;
using PHILOBMCore.Models;

namespace PHILOBMDatabase.Repositories;

public class CarRepository : BaseRepository<Models.Car>, ICarRepository
{
    public CarRepository(PhiloBMContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public async Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId)
    {
        var cars =  await _context.Cars.Where(car => car.ClientId == clientId).ToListAsync();
        
        return _mapper.Map<ICollection<Models.Car>, ICollection<Car>>(cars) ;
    }

    public async Task<Car?> GetCarByIdWithServicesAsync(int carId)
    {
        var car =  await _context.Cars
            .Include(c => c.Services) // Inclut les voitures associées
            .FirstOrDefaultAsync(c => c.Id == carId);

        return car != null ? _mapper.Map<Models.Car, Car>(car) : null;
    }

}
