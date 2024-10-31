using Microsoft.AspNetCore.Mvc;
using PHILOBMCore.Models;

namespace PHILOBMDatabase.Repositories.Interfaces;

public interface IInvoiceRepository : IBaseRepository<Models.Invoice>
{
    Task<IEnumerable<Invoice>> GetInvoicesByClientIdAsync(int selectedClientId);

}