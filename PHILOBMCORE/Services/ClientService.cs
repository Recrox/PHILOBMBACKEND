using Microsoft.EntityFrameworkCore;
using PHILOBMCore.Database;
using PHILOBMCore.Models;
using PHILOBMCore.Services.Interfaces;
namespace PHILOBMCore.Services;
public class ClientService : BaseContextService<Client>, IClientService
{
    public ClientService(PhiloBMContext context) : base(context)
    {
    }

    public async Task<Client?> GetClientByIdWithCarsAsync(int clientId)
    {
        return await _context.Clients
            .Include(c => c.Cars) // Inclut les voitures associées
            .FirstOrDefaultAsync(c => c.Id == clientId);
    }

    public bool IsClientValid()
    {
        //if (!string.IsNullOrWhiteSpace(NomTextBox.Text) && NomTextBox.Text.Length < 3)
        //    return false;

        //if (!IsPhoneNumberValid(TelephoneTextBox.Text))
        //    return false;

        //return IsAnyFieldValid();
        return true;
    }

    private bool IsPhoneNumberValid(string phoneNumber)
    {
        return !string.IsNullOrWhiteSpace(phoneNumber) &&
               phoneNumber.All(char.IsDigit) &&
               phoneNumber.Length >= 7; // Exemple de longueur minimale pour un numéro de téléphone
    }

    //private bool IsAnyFieldValid()
    //{
    //    return !string.IsNullOrWhiteSpace(PrenomTextBox.Text) ||
    //            !string.IsNullOrWhiteSpace(NomTextBox.Text) ||
    //           !string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
    //           !string.IsNullOrWhiteSpace(TelephoneTextBox.Text) ||
    //           !string.IsNullOrWhiteSpace(AdresseTextBox.Text);
    //}
}
