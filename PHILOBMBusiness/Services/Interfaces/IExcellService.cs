using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services.Interfaces;

public interface IExcellService
{
    void CreerExcelAsync(Invoice invoice);
}