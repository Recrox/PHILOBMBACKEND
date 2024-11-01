using Microsoft.AspNetCore.Mvc;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;

namespace PHILOBMBAPI.Controllers;

public class ClientController : BaseController<Client , IClientService>
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService) : base(clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{clientId}")]
    public async Task<ActionResult<Client>> GetClientDetails(int clientId)
    {
        var client = await _clientService.GetClientByIdWithCarsAsync(clientId);
        return HandleResult(client, "Client non trouvé.");
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportDatabaseToJson()
    {
        var export = await _clientService.ExportDatabaseToJson();
        return Ok(export);
    }

    //[HttpGet]
    //public override async Task<ActionResult<IEnumerable<Car>>> GetAll()
    //{
    //    // Optionnel : Retourner une réponse spécifique ou lancer une exception
    //    return NotFound("Cet endpoint n'est pas disponible pour les voitures.");
    //}
}
