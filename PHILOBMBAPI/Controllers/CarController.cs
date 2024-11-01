using Microsoft.AspNetCore.Mvc;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;

namespace PHILOBMBAPI.Controllers;

public class CarController : BaseController<Car, ICarService>
{
    private readonly ICarService _carService;

    public CarController(ICarService carService) : base(carService)
    {
        _carService = carService;
    }

    [HttpGet("cars-client")]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCarsWithClientAsync()
    {
        var cars = await _carService.GetAllCarsWithClientAsync();
        return HandleResult(cars, "Aucune voiture trouvée");
    }
    
    [HttpGet("{clientId}")]
    public async Task<ActionResult<IEnumerable<Car>>> GetCarsByClientId(int clientId)
    {
        var cars = await _carService.GetAllCarsByClientIdAsync(clientId);
        return HandleResult(cars, "Aucune voiture trouvée pour cet identifiant de client.");
    }

    [HttpGet("details/{carId}")]
    public async Task<ActionResult<Car>> GetCarDetails(int carId)
    {
        var car = await _carService.GetCarByIdWithServicesAsync(carId);
        return HandleResult(car, "Voiture non trouvée.");
    }
}
