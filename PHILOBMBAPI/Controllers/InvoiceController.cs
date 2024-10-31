using Microsoft.AspNetCore.Mvc;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore.Models;

namespace PHILOBMBAPI.Controllers;

[Route("api/[controller]")]
    public class InvoiceController : BaseController<Invoice,IInvoiceService>
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService) : base(invoiceService) 
        {
            _invoiceService = invoiceService;
        }

    [HttpGet("pdf/{invoiceId}")]
    public async Task<ActionResult<byte[]>> CreerPDFAsync(int invoiceId)
    {
        var fileContentPdf = await _invoiceService.CreerPDFAsync(invoiceId);
        if (fileContentPdf == null) return this.NotFound();
        return File(fileContentPdf.FileContents, fileContentPdf.ContentType, fileContentPdf.FileDownloadName);
    }

    //[HttpGet("pdf/{invoiceId}")]
    //public async Task<IActionResult> CreerPDFAsync(int invoiceId)
    //{
    //    var fileContentPdf = await _invoiceService.CreerPDFAsync(invoiceId);
    //    if (fileContentPdf == null) return NotFound();

    //    return Ok(fileContentPdf); // Retournez un objet JSON
    //}

    [HttpGet("mock")]
        public async Task<ActionResult<List<Invoice>>> LoadMockInvoices()
        {
            var invoices =  _invoiceService.LoadMockInvoices();
            if (invoices == null) return this.NotFound();
            return this.Ok(invoices);
        }


    //[HttpGet("{invoiceId}")]
    //public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoicesByClientIdAsync(int invoiceId)
    //{
    //    var invoice = await _invoiceService.GetInvoicesByClientIdAsync(invoiceId);
    //    return HandleResult(invoice, "Facture non trouvée.");
    //}
}

