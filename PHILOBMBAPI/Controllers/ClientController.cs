using Microsoft.AspNetCore.Mvc;
using PHILOBMBusiness.Models;
using PHILOBMBusiness.Services.Interfaces;

namespace PHILOBMBAPI.Controllers;

[Route("api/[controller]")]
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
}
