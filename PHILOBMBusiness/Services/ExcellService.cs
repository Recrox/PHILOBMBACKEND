using OfficeOpenXml.Style;
using OfficeOpenXml;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore;
using PHILOBMCore.Models;
using Configuration.ConstantsSettings;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;

namespace PHILOBMBusiness.Services;

public class ExcellService : IExcellService
{
    public void CreerExcelAsync(Invoice invoice)
    {
        string directoryPath = "Factures";
        Outils.CréerDossierSiInexistant(directoryPath);

        // Définir le contexte de licence
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Utiliser Commercial si vous avez une licence

        // Créer un nouveau package Excel
        using (var package = new ExcelPackage())
        {
            // Ajouter une nouvelle feuille
            var worksheet = package.Workbook.Worksheets.Add("Facture");

            // Définir le titre
            worksheet.Cells[1, 1].Value = "Facture PHILO B.M";
            worksheet.Cells[1, 1].Style.Font.Size = 20;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Adresse du garage
            string[] garageAddressLines = { "Rue Champ Courtin 16", "7522 Marquain", "0473/95.10.03" };
            int row = 3; // Ligne à partir de laquelle commencer à écrire les adresses
            foreach (var line in garageAddressLines)
            {
                worksheet.Cells[row++, 1].Value = line;
            }

            // Informations de la facture
            worksheet.Cells[row, 1].Value = $"Date : {DateTime.Now:dd/MM/yyyy HH:mm}";
            worksheet.Cells[row + 1, 1].Value = $"Facture n° : {invoice.Id}";
            worksheet.Cells[row + 2, 1].Value = $"Adresse client : {invoice.Client.Address}";

            // Informations du client
            row += 5; // Décalage pour les informations client
            worksheet.Cells[row++, 1].Value = $"Client: {invoice.Client.FirstName} {invoice.Client.LastName}";

            // Tableau des services
            row += 2; // Décalage pour le tableau des services
            worksheet.Cells[row, 1].Value = "Unité";
            worksheet.Cells[row, 2].Value = "Description";
            worksheet.Cells[row, 3].Value = "Prix";
            worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;

            foreach (var service in invoice.Services)
            {
                row++;
                worksheet.Cells[row, 1].Value = service.Units;
                worksheet.Cells[row, 2].Value = service.Description;
                worksheet.Cells[row, 3].Value = service.CalculateCost();
            }

            // Total
            row += 2; // Décalage pour le total
            worksheet.Cells[row, 2].Value = "Total :";
            worksheet.Cells[row, 3].Value = invoice.CalculSum();
            worksheet.Cells[row, 2, row, 3].Style.Font.Bold = true;

            // Message de paiement
            row += 2; // Décalage pour le message de paiement
            worksheet.Cells[row, 1].Value = "A verser sur le numéro de compte BE51 1262 0722 0675";
            worksheet.Cells[row, 1].Style.Font.Italic = true;

            // Ajuster la largeur des colonnes
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();

            // Sauvegarder le document Excel
            SauvegarderExcel(package, directoryPath, invoice);
            //return this.SaveExcell(invoice,document);
        }
    }

    private FileContentResult SaveExcell(Invoice invoice, PdfDocument document)
    {
        using (var stream = new MemoryStream())
        {
            document.Save(stream, false);
            var fileContents = stream.ToArray();
            var fileName = $"Facture_{invoice.Client.FirstName}_{invoice.Client.LastName}_{invoice.Date:yyyyMMdd_HHmmss}.xls";
            // Retourner le PDF en tant que FileContentResult
            return new FileContentResult(fileContents, Constants.MimeTypePDF)
            {
                FileDownloadName = fileName // Nom du fichier à télécharger
            };
        }
    }

    private void SauvegarderExcel(ExcelPackage package, string directoryPath, Invoice invoice, bool useClientNameAndDate = true)
    {
        string fileName;

        if (useClientNameAndDate)
        {
            // Nom de fichier avec le prénom, le nom du client et la date
            fileName = $"Facture_{invoice.Client.FirstName}_{invoice.Client.LastName}_{invoice.Date:yyyyMMdd_HHmmss}.xlsx";
        }
        else
        {
            // Nom de fichier avec l'ID de la facture
            fileName = $"Facture_{invoice.Id}.xlsx";
        }

        // Chemin du dossier de téléchargement
        directoryPath = Constants.DownloadPath;

        // Chemin du dossier "Factures" à l'intérieur du dossier de téléchargement
        string facturesDirectory = Path.Combine(directoryPath, "Factures");

        string filePath = Path.Combine(facturesDirectory, fileName);
        FileInfo excelFile = new FileInfo(filePath);
        package.SaveAs(excelFile);
    }
}
