using PHILOBMCore.Models;

namespace PHILOBMCore.Services.Interfaces;

public interface IInvoiceService : IBaseContextService<Invoice>
{
    void CreerPDF(Invoice invoice);
    void CreerExcel(Invoice invoice);
    Task<IEnumerable<Invoice>> GetInvoicesByClientIdAsync(int selectedClientId);
}