using Configuration.ConstantsSettings;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PHILOBMBusiness.Services.Interfaces;
using PHILOBMCore;
using PHILOBMCore.Models;

namespace PHILOBMBusiness.Services;
public class PdfService : IPdfService
{
    public FileContentResult CreatePdfAsync(Invoice invoice)
    {
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
}
