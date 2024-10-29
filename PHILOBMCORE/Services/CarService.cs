using Microsoft.EntityFrameworkCore;
using PHILOBMCore.Database;
using PHILOBMCore.Models;
using PHILOBMCore.Services.Interfaces;

namespace PHILOBMCore.Services;

public class CarService : BaseContextService<Car>, ICarService
{
    public CarService(PhiloBMContext context) : base(context)
    {

    }

    public async Task<ICollection<Car>> GetAllCarsByClientIdAsync(int clientId)
    {
        return await _context.Cars.Where(car => car.ClientId == clientId).ToListAsync();
    }

    public async Task<Car?> GetCarByIdWithServicesAsync(int carId)
    {
        return await _context.Cars
            .Include(c => c.Services) // Inclut les voitures associées
            .FirstOrDefaultAsync(c => c.Id == carId);
    }

}
