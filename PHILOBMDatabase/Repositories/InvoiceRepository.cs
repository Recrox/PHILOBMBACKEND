using Microsoft.EntityFrameworkCore;
using PHILOBMDatabase.Repositories.Interfaces;
using PHILOBMDatabase.Database;
using AutoMapper;
using PHILOBMCore.Models;

namespace PHILOBMDatabase.Repositories;

public class InvoiceRepository : BaseRepository<Invoice,Models.Invoice>, IInvoiceRepository
{
    public InvoiceRepository(PhiloBMContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByClientIdAsync(int selectedClientId)
    {
        var invoices = await _context.Invoices
            .Include(invoice => invoice.Client) // Inclut les détails du client si nécessaire
            .Where(invoice => invoice.Client.Id == selectedClientId) // Assurez-vous que Client est correctement référencé
            .ToListAsync(); // Exécute la requête et retourne la liste des factures

        return _mapper.Map<ICollection<Models.Invoice>, ICollection<Invoice>>(invoices);
    }

}