using Microsoft.AspNetCore.Mvc;
using PHILOBMCore.Models;

namespace PHILOBMCore.Services.Interfaces;

public interface IInvoiceService : IBaseContextService<Invoice>
{
    Task<FileContentResult?> CreerPDFAsync(int invoiceId);
    Task CreerExcelAsync(Invoice invoice);
    Task<IEnumerable<Invoice>> GetInvoicesByClientIdAsync(int selectedClientId);
    List<Invoice> LoadMockInvoices();
}