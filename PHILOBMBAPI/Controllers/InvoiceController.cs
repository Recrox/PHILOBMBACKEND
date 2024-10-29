using Microsoft.AspNetCore.Mvc;
using PHILOBMCore.Models;
using PHILOBMCore.Services.Interfaces;

namespace PHILOBMBAPI.Controllers;

[Route("api/[controller]")]
    public class InvoiceController : BaseController<Invoice,IInvoiceService>
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService) : base(invoiceService) 
        {
            _invoiceService = invoiceService;
        }

        //[HttpGet("{invoiceId}")]
        //public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoicesByClientIdAsync(int invoiceId)
        //{
        //    var invoice = await _invoiceService.GetInvoicesByClientIdAsync(invoiceId);
        //    return HandleResult(invoice, "Facture non trouvée.");
        //}
    }

