using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PHILOBMCore.Database;
using PHILOBMCore.Models;
using PHILOBMCore.Services.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PHILOBMCore.ConstantsSettings;
using Microsoft.AspNetCore.Mvc;

namespace PHILOBMCore.Services;

public class InvoiceService : BaseContextService<Invoice>, IInvoiceService
{
    public InvoiceService(PhiloBMContext context) : base(context)
    {

    }

    public async Task<FileContentResult?> CreerPDFAsync(int invoiceId)
    {
        var invoice = await base.GetByIdAsync(invoiceId);
        invoice =  this.LoadMockInvoices().FirstOrDefault(i => i.Id == 4);
        if (invoice == null) return null;

        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        double margin = 40;
        double yPoint = margin;
        double adresseHeight;

        DessinerTitre(gfx, page, ref yPoint);
        DessinerAdresseGarage(gfx, page, ref yPoint, margin, out adresseHeight);
        DessinerInformationsDroite(gfx, page, invoice, yPoint, margin);
        DessinerTVA(gfx, page, ref yPoint, margin);
        DessinerInformationsClient(gfx, page, invoice, ref yPoint, margin);
        DessinerTableauPlaqueEtKilometrage(gfx, page, invoice, ref yPoint, margin);
        DessinerTableauServices(gfx, document, page, invoice, ref yPoint, margin);

        SauvegarderDocument(document, invoice);

        // Sauvegarder le document dans un MemoryStream
        return SavePdf(invoice, document);
    }

    private FileContentResult SavePdf(Invoice invoice, PdfDocument document)
    {
        using (var stream = new MemoryStream())
        {
            document.Save(stream, false);
            var fileContents = stream.ToArray();
            var fileName = $"Facture_{invoice.Client.FirstName}_{invoice.Client.LastName}_{invoice.Date:yyyyMMdd_HHmmss}.pdf";
            // Retourner le PDF en tant que FileContentResult
            return new FileContentResult(fileContents, Constants.MimeTypePDF)
            {
                FileDownloadName = fileName // Nom du fichier à télécharger
            };
        }
    }

    private void DessinerTitre(XGraphics gfx, PdfPage page, ref double yPoint)
    {
        gfx.DrawString("Facture PHILO B.M", new XFont("Verdana", 20, XFontStyle.Bold), XBrushes.Black,
            new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopCenter);
        yPoint += 50;
    }

    private void DessinerAdresseGarage(XGraphics gfx, PdfPage page, ref double yPoint, double margin, out double adresseHeight)
    {
        string[] garageAddressLines = { "Rue Champ Courtin 16", "7522 Marquain", "0473/95.10.03" };
        adresseHeight = 20 * garageAddressLines.Length; // Calculer la hauteur totale de l'adresse

        foreach (var line in garageAddressLines)
        {
            gfx.DrawString(line, new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
        }
    }

    private void DessinerInformationsDroite(XGraphics gfx, PdfPage page, Invoice invoice, double yPoint, double margin)
    {
        double rightMargin = page.Width - margin;
        string dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        double initialYPoint = yPoint; // Conserver le yPoint initial

        //// Charger le logo de BMW
        //string logoPath = Path.Combine("Assets", "bmw_logo.png");
        //if (File.Exists(logoPath))
        //{
        //    XImage logo = XImage.FromFile(logoPath);
        //    gfx.DrawImage(logo, rightMargin - 150, yPoint - 40, 150, 40); // Ajustez la position et la taille selon vos besoins
        //    yPoint += 40; // Ajustez yPoint pour laisser de l'espace pour l'image
        //}

        gfx.DrawString($"Date : {dateTime}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(rightMargin - 150, yPoint, 150, 20), XStringFormats.TopRight);
        yPoint += 20;

        gfx.DrawString($"Facture n° : {invoice.Id}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(rightMargin - 150, yPoint, 150, 20), XStringFormats.TopRight);
        yPoint += 20;

        gfx.DrawString($"Adresse client : {invoice.Client.Address}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(rightMargin - 150, yPoint, 150, 20), XStringFormats.TopRight);
        yPoint += 20;

        // Alignez la hauteur des informations à droite avec celle de l'adresse du garage
        yPoint = initialYPoint; // Réinitialiser yPoint à la valeur initiale
    }



    private void DessinerTVA(XGraphics gfx, PdfPage page, ref double yPoint, double margin)
    {
        gfx.DrawString("TVA : BE 10 10 41 54 45", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
        yPoint += 40;
    }

    private void DessinerInformationsClient(XGraphics gfx, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        gfx.DrawString($"Client: {invoice.Client.FullName}", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, page.Height), XStringFormats.TopLeft);
        yPoint += 20;
    }

    private void DessinerTableauPlaqueEtKilometrage(XGraphics gfx, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        // Tableau pour le numéro de plaque et kilométrage
        yPoint += 20;
        double cellHeight = 20;
        double tableWidth = page.Width - 2 * margin;
        double cellWidth = tableWidth / 2;

        gfx.DrawRectangle(XPens.Black, margin, yPoint, cellWidth, cellHeight);
        gfx.DrawRectangle(XPens.Black, margin + cellWidth, yPoint, cellWidth, cellHeight);
        gfx.DrawString("Numéro de plaque:", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString("Kilométrage:", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + cellWidth, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        yPoint += cellHeight;

        gfx.DrawRectangle(XPens.Black, margin, yPoint, cellWidth, cellHeight);
        gfx.DrawRectangle(XPens.Black, margin + cellWidth, yPoint, cellWidth, cellHeight);
        gfx.DrawString(invoice.Car.LicensePlate ?? "Non spécifié", new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString(invoice.Car.Mileage.ToString(), new XFont("Verdana", 12), XBrushes.Black,
            new XRect(margin + cellWidth, yPoint, cellWidth, cellHeight), XStringFormats.Center);
        yPoint += 30;
    }

    private void DessinerTableauServices(XGraphics gfx, PdfDocument document, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        // Largeurs pour chaque colonne
        double unitColWidth = 50; // Largeur pour la colonne "Unité"
        double priceColWidth = 100; // Largeur pour la colonne "Prix" (suffisante pour 12 chiffres)
        double descriptionPadding = 10; // Espace au début de la colonne "Description"
        double pricePadding = 10; // Espace à droite de la colonne "Prix"
        double descriptionColWidth = (page.Width - 2 * margin - unitColWidth - priceColWidth - descriptionPadding - pricePadding); // Largeur restante pour "Description"
        double cellHeight = 20;

        // Dessiner les en-têtes du tableau
        DessinerRectangle(gfx, margin, yPoint, unitColWidth, cellHeight);
        DessinerRectangle(gfx, margin + unitColWidth, yPoint, descriptionColWidth + descriptionPadding, cellHeight);
        DessinerRectangle(gfx, margin + unitColWidth + descriptionColWidth + descriptionPadding, yPoint, priceColWidth + pricePadding, cellHeight);

        gfx.DrawString("Unité", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin, yPoint, unitColWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString("Description", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + unitColWidth + descriptionPadding, yPoint, descriptionColWidth, cellHeight), XStringFormats.Center);
        gfx.DrawString("Prix", new XFont("Verdana", 12, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin + unitColWidth + descriptionColWidth + descriptionPadding, yPoint, priceColWidth, cellHeight), XStringFormats.Center);
        yPoint += cellHeight;

        foreach (var service in invoice.Services)
        {
            // Vérifier si l'on doit ajouter une nouvelle page
            if (yPoint + cellHeight > page.Height - 50) // Laisser une marge de 50 points en bas
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPoint = 40; // Réinitialiser yPoint pour la nouvelle page
            }

            DessinerRectangle(gfx, margin, yPoint, unitColWidth, cellHeight);
            DessinerRectangle(gfx, margin + unitColWidth, yPoint, descriptionColWidth + descriptionPadding, cellHeight);
            DessinerRectangle(gfx, margin + unitColWidth + descriptionColWidth + descriptionPadding, yPoint, priceColWidth + pricePadding, cellHeight);

            gfx.DrawString(service.Units.ToString(), new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin, yPoint, unitColWidth, cellHeight), XStringFormats.Center);
            gfx.DrawString(service.Description, new XFont("Verdana", 12), XBrushes.Black,
                new XRect(margin + unitColWidth + descriptionPadding, yPoint, descriptionColWidth, cellHeight), XStringFormats.CenterLeft);
            gfx.DrawString($"{service.CalculateCost():F2} €", new XFont("Verdana", 12), XBrushes.Black, // Format avec deux décimales
                new XRect(margin + unitColWidth + descriptionColWidth + descriptionPadding, yPoint, priceColWidth, cellHeight), XStringFormats.CenterRight);
            yPoint += cellHeight;
        }

        // Appel à DessinerTotal après le tableau
        DessinerTotal(gfx, document, page, invoice, ref yPoint, margin);

        // Appel à DessinerMessagePaiement après le total
        DessinerMessagePaiement(gfx, document, page, ref yPoint, margin);
    }



    private void DessinerTotal(XGraphics gfx, PdfDocument document, PdfPage page, Invoice invoice, ref double yPoint, double margin)
    {
        yPoint += 10; // Ajout d'un espace avant le total
        double totalHeight = 20; // Hauteur de la zone de texte du total

        // Vérification si yPoint dépasse la hauteur de la page
        if (yPoint + totalHeight + 30 > page.Height - 50) // Laisser une marge de 50 points en bas
        {
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            yPoint = margin; // Réinitialiser yPoint pour la nouvelle page
        }

        // Placer le total à la fin de la page
        gfx.DrawString($"Total: {invoice.CalculSum()} €", new XFont("Verdana", 14, XFontStyle.Bold), XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, totalHeight), XStringFormats.CenterRight);
        yPoint += totalHeight + 10; // Ajuster l'espacement après le total
    }

    private void DessinerMessagePaiement(XGraphics gfx, PdfDocument document, PdfPage page, ref double yPoint, double margin)
    {
        double messageHeight = 40; // Hauteur de la zone de texte pour le message de paiement

        // Vérification si yPoint dépasse la hauteur de la page
        if (yPoint + messageHeight > page.Height - 50) // Laisser une marge de 50 points en bas
        {
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            yPoint = margin; // Réinitialiser yPoint pour la nouvelle page
        }

        yPoint += 10; // Ajouter un espace avant le message de paiement
                      // Placer le message de paiement à la fin de la page avec une police en italique
        gfx.DrawString("A verser sur le numéro de compte BE51 1262 0722 0675",
            new XFont("Verdana", 12, XFontStyle.Italic), // Changement ici
            XBrushes.Black,
            new XRect(margin, yPoint, page.Width - 2 * margin, messageHeight),
            XStringFormats.TopLeft);
        yPoint += messageHeight + 10; // Ajustez si nécessaire pour un espacement ultérieur
    }

    private void DessinerRectangle(XGraphics gfx, double x, double y, double width, double height)
    {
        gfx.DrawRectangle(XPens.Black, x, y, width, height);
    }
    private void SauvegarderDocument(PdfDocument document, Invoice invoice, bool useClientNameAndDate = true)
    {
        string fileName;

        if (useClientNameAndDate)
        {
            // Nom de fichier avec le prénom, le nom du client et la date
            fileName = $"Facture_{invoice.Client.FirstName}_{invoice.Client.LastName}_{invoice.Date:yyyyMMdd_HHmmss}.pdf";
        }
        else
        {
            // Nom de fichier avec l'ID de la facture
            fileName = $"Facture_{invoice.Id}.pdf";
        }

        // Chemin du dossier de téléchargement
        string directoryPath = Constants.DownloadPath;

        // Chemin du dossier "Factures" à l'intérieur du dossier de téléchargement
        string facturesDirectory = Path.Combine(directoryPath, "Factures");

        // Créer le dossier "Factures" s'il n'existe pas
        Outils.CréerDossierSiInexistant(facturesDirectory);

        // Chemin complet du fichier PDF
        string filePath = Path.Combine(facturesDirectory, fileName);

        // Sauvegarder le document PDF
        document.Save(filePath);
        document.Close();
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesByClientIdAsync(int selectedClientId)
    {
        var invoices = await _context.Invoices
            .Include(invoice => invoice.Client) // Inclut les détails du client si nécessaire
            .Where(invoice => invoice.Client.Id == selectedClientId) // Assurez-vous que Client est correctement référencé
            .ToListAsync(); // Exécute la requête et retourne la liste des factures

        return invoices;
    }



    public async Task CreerExcelAsync(Invoice invoice)
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

    public List<Invoice> LoadMockInvoices()
    {
        List<Invoice> Invoices = new List<Invoice>();
        // Créez une liste de factures fictives avec des adresses pour chaque client
        Invoices.Add(new Invoice
        {
            Id = 1,
            Client = new Client
            {
                Id = 1,
                LastName = "Dupont",
                FirstName = "Arno",
                Address = "12 Rue de la Paix, 75002 Paris, France"
            },
            Car = new Car { LicensePlate = "ABC 123", Mileage = 56023 },
            Date = DateTime.Now.AddDays(-10),
            Services = new List<Service>
        {
            new Service { Description = "Changement d'huile", Price = 50, Units = 1 },
            new Service { Description = "Réparation de frein", Price = 120, Units = 2 },
            new Service { Description = "Vérification des niveaux", Price = 30, Units = 1 },
            new Service { Description = "Changement de filtre à air", Price = 40, Units = 1 },
            new Service { Description = "Alignement des roues", Price = 70, Units = 1 }
        }
        });

        Invoices.Add(new Invoice
        {
            Id = 2,
            Client = new Client
            {
                Id = 2,
                LastName = "Martin",
                FirstName = "Julie",
                Address = "48 Avenue des Champs-Élysées, 75008 Paris, France"
            },
            Car = new Car { LicensePlate = "XYZ 789", Mileage = 56023 },
            Date = DateTime.Now.AddDays(-5),
            Services = new List<Service>
        {
            new Service { Description = "Contrôle technique", Price = 80, Units = 1 },
            new Service { Description = "Remplacement de pneus", Price = 300, Units = 4 },
            new Service { Description = "Réparation de la climatisation", Price = 150, Units = 1 },
            new Service { Description = "Changement de plaquettes de frein", Price = 90, Units = 2 },
            new Service { Description = "Vérification des freins", Price = 50, Units = 1 }
        }
        });

        Invoices.Add(new Invoice
        {
            Id = 3,
            Client = new Client
            {
                Id = 3,
                LastName = "Bernard",
                FirstName = "Luc",
                Address = "15 Place de l'Hôtel de Ville, 69001 Lyon, France"
            },
            Car = new Car { LicensePlate = "LMN 456", Mileage = 56023 },
            Date = DateTime.Now.AddDays(-2),
            Services = new List<Service>
        {
            new Service { Description = "Révision complète", Price = 150, Units = 1 },
            new Service { Description = "Changement de batterie", Price = 120, Units = 1 },
            new Service { Description = "Nettoyage intérieur et extérieur", Price = 80, Units = 1 },
            new Service { Description = "Diagnostic électronique", Price = 60, Units = 1 },
            new Service { Description = "Changement de liquide de refroidissement", Price = 50, Units = 1 }
        }
        });

        // Ajout d'une facture avec un grand nombre de services
        Invoices.Add(new Invoice
        {
            Id = 4,
            Client = new Client
            {
                Id = 4,
                LastName = "Dubois",
                FirstName = "Claire",
                Address = "25 Rue de Rivoli, 75001 Paris, France"
            },
            Car = new Car { LicensePlate = "JKL 890", Mileage = 56023 },
            Date = DateTime.Now.AddDays(-1),
            Services = new List<Service>
        {
            new Service { Description = "Changement d'huile", Price = 50, Units = 1 },
            new Service { Description = "Réparation de frein", Price = 120, Units = 2 },
            new Service { Description = "Vérification des niveaux", Price = 30, Units = 1 },
            new Service { Description = "Changement de filtre à air", Price = 40, Units = 1 },
            new Service { Description = "Alignement des roues", Price = 70, Units = 1 },
            new Service { Description = "Remplacement des balais d'essuie-glace", Price = 20, Units = 2 },
            new Service { Description = "Vérification de la batterie", Price = 25, Units = 1 },
            new Service { Description = "Contrôle de l'éclairage", Price = 15, Units = 1 },
            new Service { Description = "Remplacement de courroie", Price = 200, Units = 1 },
            new Service { Description = "Réglage des phares", Price = 40, Units = 1 },
            new Service { Description = "Changement de liquide de frein", Price = 60, Units = 1 },
            new Service { Description = "Service de climatisation", Price = 100, Units = 1 },
            new Service { Description = "Mise à jour du logiciel du véhicule", Price = 75, Units = 1 },
            new Service { Description = "Inspection des freins", Price = 50, Units = 1 },
            new Service { Description = "Changement de bougies d'allumage", Price = 80, Units = 4 },
            new Service { Description = "Changement de filtre à huile", Price = 30, Units = 1 },
            new Service { Description = "Diagnostic de performance", Price = 90, Units = 1 },
            new Service { Description = "Vérification des niveaux de liquide", Price = 20, Units = 1 },
            new Service { Description = "Contrôle des pneus", Price = 50, Units = 1 },
            new Service { Description = "Inspection de sécurité", Price = 100, Units = 1 },
            new Service { Description = "Vérification des amortisseurs", Price = 60, Units = 1 },
            new Service { Description = "Changement de filtre à carburant", Price = 45, Units = 1 },
            new Service { Description = "Réparation du système d'échappement", Price = 120, Units = 1 },
            new Service { Description = "Contrôle de l'ABS", Price = 70, Units = 1 },
            new Service { Description = "Vérification de la direction", Price = 50, Units = 1 },
            new Service { Description = "Changement de pneus", Price = 80, Units = 4 },
            new Service { Description = "Réparation de la climatisation", Price = 150, Units = 1 },
            new Service { Description = "Révision complète du moteur", Price = 300, Units = 1 },
            new Service { Description = "Changement de liquide de refroidissement", Price = 50, Units = 1 },
            new Service { Description = "Contrôle de l'état des freins", Price = 40, Units = 1 },
            new Service { Description = "Réparation de la transmission", Price = 200, Units = 1 },
            new Service { Description = "Réparation de la suspension", Price = 150, Units = 1 },
            new Service { Description = "Changement de filtre à particules", Price = 100, Units = 1 },
            new Service { Description = "Alignement des roues", Price = 70, Units = 1 },
            new Service { Description = "Mise à jour du GPS", Price = 40, Units = 1 }
        }
        });
        return Invoices;
    }

}