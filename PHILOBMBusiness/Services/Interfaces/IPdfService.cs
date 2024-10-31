using Microsoft.AspNetCore.Mvc;
using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services.Interfaces;

public interface IPdfService
{
    FileContentResult CreatePdfAsync(Invoice invoice);
}