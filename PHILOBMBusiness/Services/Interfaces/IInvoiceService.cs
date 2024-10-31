using Microsoft.AspNetCore.Mvc;
using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services.Interfaces;

public interface IInvoiceService : IBaseService<Invoice>
{
    Task<FileContentResult?> CreerPDFAsync(int invoiceId);
    Task CreerExcelAsync(Invoice invoice);
    Task<IEnumerable<Invoice>> GetInvoicesByClientIdAsync(int selectedClientId);
    List<Invoice> LoadMockInvoices();
}